using HarmonyLib;

using Il2CppSLZ.Marrow;

using BoneLib;

using UnityEngine;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(AnimationRig))]
public static class AnimationRigPatches
{
    
    [HarmonyPatch(typeof(AnimationRig), nameof(AnimationRig.OnEarlyUpdate))]
    [HarmonyPostfix]
    public static void OnEarlyUpdate(AnimationRig __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (__instance.manager != Player.RigManager)
            return;
        
        __instance.eyeGaze = new Vector4(Tracking.Data.Eye.Combined().Gaze.x, Tracking.Data.Eye.Combined().Gaze.y, 1f, 2f);
    }
}