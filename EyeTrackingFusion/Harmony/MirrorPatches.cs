using EyeTracking.MarrowSDK;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using LabFusion.Entities;
using LabFusion.MonoBehaviours;
using LabFusion.Player;

namespace EyeTracking.Fusion.Harmony;

[HarmonyPatch(typeof(Mirror))]
public class MirrorPatches
{
    [HarmonyPatch(typeof(Mirror), nameof(Mirror.WriteTransforms))]
    [HarmonyPostfix]
    public static void WriteTransforms(Mirror __instance)
    {
        var identifier = __instance.GetComponent<MirrorIdentifier>();
        if (identifier == null || identifier.id == PlayerIDManager.LocalSmallID)
            return;
        
        if (!NetworkPlayerManager.TryGetPlayer(identifier.id, out var networkPlayer))
            return;
        
        if (!EyeSyncer.TryGet(networkPlayer, out var eyeSyncer))
            return;
        
        if (eyeSyncer.AvatarEyeGazeDescriptor == null)
            return;
        
        var mirrorEyeDescriptor = __instance.Reflection.GetComponent<AvatarEyeGazeDescriptor>();
        if (mirrorEyeDescriptor == null)
            return;
        
        mirrorEyeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(eyeSyncer.AvatarEyeGazeDescriptor.LeftEyeBlinkShapeKey.Get(), eyeSyncer.CurrentLeftBlink * 100f);
        mirrorEyeDescriptor?.SkinnedMeshRenderer?.Get()?.SetBlendShapeWeight(eyeSyncer.AvatarEyeGazeDescriptor.RightEyeBlinkShapeKey.Get(), eyeSyncer.CurrentRightBlink * 100f);
    }
}