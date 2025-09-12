using BoneLib;
using EyeTracking.MarrowSDK;
using Il2CppSLZ.Marrow;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking;

public static class EyeSolver
{
    // TODO: Implement blinking logic here
    
    internal static AvatarEyeGazeDescriptor? CurrentEyeGazeDescriptor { get; private set; }
    internal static int LeftBlinkIndex { get; private set; } = -1;
    internal static int RightBlinkIndex { get; private set; } = -1;
    
    // variables used internally for smoothing
    public static readonly float SmoothingSpeed = 35f;
    
    private static float _currentLeftBlink = 0f;
    private static float _currentRightBlink = 0f;

    public static void OnAvatarSwap(Avatar avatar)
    {
        CurrentEyeGazeDescriptor = null;
        LeftBlinkIndex = -1;
        RightBlinkIndex = -1;
        
        CurrentEyeGazeDescriptor = avatar.GetComponent<AvatarEyeGazeDescriptor>();

        if (CurrentEyeGazeDescriptor == null)
            return;
        
        LeftBlinkIndex = CurrentEyeGazeDescriptor.LeftEyeBlinkShapeKey.Get();
        RightBlinkIndex = CurrentEyeGazeDescriptor.RightEyeBlinkShapeKey.Get();
        
#if DEBUG
        Core.Instance.LoggerInstance.Msg($"Swapped to avatar: {avatar.name} : LeftBlinkIndex={LeftBlinkIndex} : RightBlinkIndex={RightBlinkIndex}");
#endif
    }

    public static void UpdateBlink()
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        // having eye gaze descriptors is fine, but lets not update the blinking.
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        if (CurrentEyeGazeDescriptor == null || CurrentEyeGazeDescriptor.SkinnedMeshRenderer.Get() == null || LeftBlinkIndex == -1 || RightBlinkIndex == -1)
            return;

        var targetLeftBlink = 1f - Tracking.Data.Eye.Left.Openness;
        var targetRightBlink = 1f - Tracking.Data.Eye.Right.Openness;
        
        _currentLeftBlink = Mathf.Lerp(_currentLeftBlink, targetLeftBlink, SmoothingSpeed * Time.deltaTime);
        _currentRightBlink = Mathf.Lerp(_currentRightBlink, targetRightBlink, SmoothingSpeed * Time.deltaTime);
        
        CurrentEyeGazeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(LeftBlinkIndex, _currentLeftBlink * 100f);
        CurrentEyeGazeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(RightBlinkIndex, _currentRightBlink * 100f);
    }
}