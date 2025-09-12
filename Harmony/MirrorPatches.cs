using EyeTracking.MarrowSDK;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(Mirror))]
public class MirrorPatches
{
    private static float _currentLeftBlink = 0f;
    private static float _currentRightBlink = 0f;

    [HarmonyPatch(typeof(Mirror), nameof(Mirror.WriteTransforms))]
    [HarmonyPostfix]
    public static void WriteTransforms(Mirror __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;

        if (__instance == null || __instance.Pointer == System.IntPtr.Zero)
            return;

        if (__instance.Reflection == null || __instance.Reflection.Pointer == System.IntPtr.Zero)
            return;

        if (!__instance.Reflection.gameObject.activeSelf)
            return;

        if (__instance.rigManager == null)
            return;
        
        if (EyeSolver.CurrentEyeGazeDescriptor == null)
            return;

        var mirrorEyeGazeDescriptor = __instance.Reflection.GetComponent<AvatarEyeGazeDescriptor>();

        var targetLeftBlink = 1f - Tracking.Data.Eye.Left.Openness;
        var targetRightBlink = 1f - Tracking.Data.Eye.Right.Openness;

        _currentLeftBlink = Mathf.Lerp(_currentLeftBlink, targetLeftBlink, EyeSolver.SmoothingSpeed * Time.deltaTime);
        _currentRightBlink = Mathf.Lerp(_currentRightBlink, targetRightBlink, EyeSolver.SmoothingSpeed * Time.deltaTime);
        
        mirrorEyeGazeDescriptor.SkinnedMeshRenderer.Get().SetBlendShapeWeight(EyeSolver.LeftBlinkIndex, _currentLeftBlink * 100f);
        mirrorEyeGazeDescriptor.SkinnedMeshRenderer.Get().SetBlendShapeWeight(EyeSolver.RightBlinkIndex, _currentRightBlink * 100f);
    }
}