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
        
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        if (__instance.manager != Player.RigManager)
            return;
        
        var gaze = Tracking.Data.Eye.Combined().Gaze;
        
        if (Tracking.Data.Eye.Left.Openness < 0.05f)
            gaze = Tracking.Data.Eye.Right.Gaze;
        else if (Tracking.Data.Eye.Right.Openness < 0.05f)
            gaze = Tracking.Data.Eye.Left.Gaze;
        
        __instance.eyeGaze = new Vector4(gaze.x, gaze.y, 1f, 2f);
    }
}