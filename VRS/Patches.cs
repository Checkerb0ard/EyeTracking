using HarmonyLib;

using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace EyeTracking.VRS;

// idk

[HarmonyPatch(typeof(VRSRenderFeature))]
public static class Patches
{
    [HarmonyPatch(nameof(VRSRenderFeature.CalculateEyeCenter))]
    [HarmonyPrefix]
    public static bool CalculateEyeCenter(VRSRenderFeature __instance, Matrix4x4 projection, ref Vector2 __result)
    {
        __result = (Tracking.Data.Eye.Combined().Gaze + Vector2.one) * 0.5f;
        return true;
    }
}