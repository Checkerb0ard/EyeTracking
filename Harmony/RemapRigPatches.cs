using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Il2CppSLZ.Marrow;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(RemapRig))]
public static class RemapRigPatches
{
    [HarmonyPatch(typeof(RemapRig), nameof(RemapRig.OnLateUpdate))]
    [HarmonyPostfix]
    public static void OnLateUpdate(RemapRig __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (__instance.manager != BoneLib.Player.RigManager)
            return;
        
        var animator = __instance.avatar?.animator;
        if (animator == null) 
            return;
        
        var leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
        var rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);
        if (leftEye == null || rightEye == null)
            return;
        
        EyeSolver.ApplyEye(leftEye, EyeType.Left);
        EyeSolver.ApplyEye(rightEye, EyeType.Right);
    }
}