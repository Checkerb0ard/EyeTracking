using HarmonyLib;
using Il2CppRealisticEyeMovements;
using Il2CppSLZ.VRMK;

namespace EyeTracking.Harmony;

[HarmonyPatch]
public class EyeAndHeadAnimatorPatches
{
    [HarmonyPatch(typeof(EyeAndHeadAnimator), nameof(EyeAndHeadAnimator.Awake))]
    [HarmonyPrefix]
    public static void Awake(EyeAndHeadAnimator __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        UnityEngine.Object.DestroyImmediate(__instance);
    }
    
    [HarmonyPatch(typeof(LookTargetController), nameof(LookTargetController.Awake))]
    [HarmonyPrefix]
    public static void Awake(LookTargetController __instance)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        UnityEngine.Object.DestroyImmediate(__instance);
    }
}