using HarmonyLib;

using Il2CppSLZ.Marrow;

using UnityEngine;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(Mirror))]
public class MirrorPatches
{
    [HarmonyPatch(nameof(Mirror.LateUpdate))]
    [HarmonyPostfix]
    public static void LateUpdate(Mirror __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        var animator = __instance?.Reflection?.animator;
        if (animator == null)
            return;

        var leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
        var rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);
        if (leftEye == null || rightEye == null)
            return;
        
        EyeSolver.ApplyEyeSimple(leftEye, Tracking.Data.Eye.Combined().Gaze);
        EyeSolver.ApplyEyeSimple(rightEye, Tracking.Data.Eye.Combined().Gaze);
    }
}