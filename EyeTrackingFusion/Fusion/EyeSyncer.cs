using EyeTracking.MarrowSDK;
using LabFusion.Entities;
using UnityEngine;
using Random = System.Random;

namespace EyeTracking.Fusion;

public class EyeSyncer
{
    private static Dictionary<NetworkPlayer, EyeSyncer> EyeSyncers = new Dictionary<NetworkPlayer, EyeSyncer>();

    internal static void Create(NetworkPlayer networkPlayer)
    {
        if (EyeSyncers.ContainsKey(networkPlayer))
            return;

        var eyeSyncer = new EyeSyncer(networkPlayer);
        EyeSyncers.Add(networkPlayer, eyeSyncer);
    }
    
    internal static void Remove(NetworkPlayer networkPlayer)
    {
        if (!EyeSyncers.ContainsKey(networkPlayer))
            return;

        EyeSyncers.Remove(networkPlayer);
    }
    
    internal static void Remove(EyeSyncer eyeSyncer)
    {
        if (eyeSyncer.NetworkPlayer == null)
            return;
        
        if (!EyeSyncers.ContainsKey(eyeSyncer.NetworkPlayer))
            return;

        EyeSyncers.Remove(eyeSyncer.NetworkPlayer);
    }
    
    internal static bool TryGet(NetworkPlayer networkPlayer, out EyeSyncer eyeSyncer)
    {
        return EyeSyncers.TryGetValue(networkPlayer, out eyeSyncer);
    }
    
    internal static void UpdateAll()
    {
        foreach (var eyeSyncer in EyeSyncers.Values)
        {
            eyeSyncer.Update();
        }
    }
    
    internal static void SetTargetGaze(NetworkPlayer networkPlayer, Vector2 targetGaze, float leftOpenness, float rightOpenness)
    {
        if (EyeSyncers.TryGetValue(networkPlayer, out var eyeSyncer))
        {
            eyeSyncer.TargetGaze = targetGaze;
            eyeSyncer.TargetLeftOpenness = leftOpenness;
            eyeSyncer.TargetRightOpenness = rightOpenness;
        }
        else
        {
            Create(networkPlayer);
            EyeSyncers[networkPlayer].TargetGaze = targetGaze;
            EyeSyncers[networkPlayer].TargetLeftOpenness = leftOpenness;
            EyeSyncers[networkPlayer].TargetRightOpenness = rightOpenness;
        }
    }

    EyeSyncer(NetworkPlayer networkPlayer)
    {
        NetworkPlayer = networkPlayer;
    }

    private Vector2 TargetGaze;
    private float TargetLeftOpenness;
    private float TargetRightOpenness;
    public AvatarEyeGazeDescriptor? AvatarEyeGazeDescriptor;
    public float CurrentLeftBlink = 0f;
    public float CurrentRightBlink = 0f;
    
    private NetworkPlayer? NetworkPlayer { get; }

    // did this to avoid the client dropping eye data and causing the eyes to snap to the center.
    private void Update()
    {
        if (NetworkPlayer?.RigRefs?.RigManager == null)
        {
            Remove(this);
        }
        
        if (NetworkPlayer?.RigRefs?.RigManager?.animationRig == null)
            return;
        
        if (AvatarEyeGazeDescriptor == null)
            AvatarEyeGazeDescriptor = NetworkPlayer?.RigRefs?.RigManager?.avatar?.gameObject?.GetComponent<AvatarEyeGazeDescriptor>();
        if (AvatarEyeGazeDescriptor?.gameObject.activeSelf == false)
            AvatarEyeGazeDescriptor = NetworkPlayer?.RigRefs?.RigManager?.avatar?.GetComponent<AvatarEyeGazeDescriptor>();
        
        //NetworkPlayer.RigRefs.RigManager.animationRig.eyeGaze = new Vector4(TargetGaze.x, TargetGaze.y, 1, 2);
        NetworkPlayer.RigRefs.RigManager.animationRig.eyeGaze = Vector4.Lerp(NetworkPlayer.RigRefs.RigManager.animationRig.eyeGaze, new Vector4(TargetGaze.x, TargetGaze.y, 1, 2), EyeSolver.SmoothingSpeed * Time.deltaTime);
        
        if (AvatarEyeGazeDescriptor == null)
            return;
        
        var targetLeftBlink = 1f - TargetLeftOpenness;
        var targetRightBlink = 1f - TargetRightOpenness;
        
        CurrentLeftBlink = Mathf.Lerp(CurrentLeftBlink, targetLeftBlink, EyeSolver.SmoothingSpeed * Time.deltaTime);
        CurrentRightBlink = Mathf.Lerp(CurrentRightBlink, targetRightBlink, EyeSolver.SmoothingSpeed * Time.deltaTime);
        
        AvatarEyeGazeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(AvatarEyeGazeDescriptor.LeftEyeBlinkShapeKey.Get(), CurrentLeftBlink * 100f);
        AvatarEyeGazeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(AvatarEyeGazeDescriptor.RightEyeBlinkShapeKey.Get(), CurrentRightBlink * 100f);
    }
}