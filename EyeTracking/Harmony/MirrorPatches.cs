using EyeTracking.MarrowSDK;
using HarmonyLib;
using Il2CppSLZ.Marrow;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(Mirror))]
internal static class MirrorPatches
{
    [HarmonyPatch(typeof(Mirror), nameof(Mirror.WriteTransforms))]
    [HarmonyPostfix]
    private static void WriteTransforms(Mirror __instance)
    {
        if (!Validate(__instance))
            return;

        if (Core.Instance.SolverManager.EyeSolver.CurrentDescriptor != null)
        {
            var mirrorEyeGazeDescriptor = __instance.Reflection.GetComponent<AvatarEyeGazeDescriptor>();
            Core.Instance.SolverManager.EyeSolver.SetBlinkWeight(mirrorEyeGazeDescriptor);
        }

        if (Core.Instance.SolverManager.FaceSolver.CurrentDescriptor != null)
        {
            var mirrorFaceDescriptor = __instance.Reflection.GetComponent<AvatarFaceDescriptor>();
            Core.Instance.SolverManager.FaceSolver.SetFaceWeight(mirrorFaceDescriptor);
        }
    }
    
    private static bool Validate(Mirror mirror)
    {
        if (!Core.Instance.PreferencesManager.Enabled.Value)
            return false;
        
        if (!Tracking.IsTracking)
            return false;
        
        if (mirror == null || mirror.Pointer == IntPtr.Zero)
            return false;
        
        if (mirror.Reflection == null || mirror.Reflection.Pointer == IntPtr.Zero)
            return false;
        
        if (!mirror.Reflection.gameObject.activeSelf)
            return false;

        if (mirror.rigManager == null)
            return false;
        
        return true;
    }
}