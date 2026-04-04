using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using UnityEngine;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(AnimationRig))]
internal static class AnimationRigPatches
{
    [HarmonyPatch(typeof(AnimationRig), nameof(AnimationRig.OnEarlyUpdate))]
    [HarmonyPostfix]
    private static void OnEarlyUpdate(AnimationRig __instance)
    {
        if (!Validate(__instance))
            return;
        
        var eye = Tracking.EyeData;
        
        var source =
            eye.Left.Openness < 0.05f ? eye.Right :
            eye.Right.Openness < 0.05f ? eye.Left :
            eye.Combined();

        float gazeX = source.GazeX;
        float gazeY = source.GazeY;

        __instance.eyeGaze = new Vector4(gazeX, gazeY, 1f, 2f);
    }
    
    private static bool Validate(AnimationRig animationRig)
    {
        if (!Core.Instance.PreferencesManager.Enabled.Value)
            return false;
        
        if (!Tracking.IsTracking)
            return false;
        
        if (!Tracking.SupportsEye)
            return false;
        
        if (animationRig.manager != Player.RigManager)
            return false;
        
        return true;
    }
}