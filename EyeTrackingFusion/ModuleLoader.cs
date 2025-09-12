using HarmonyLib;
using LabFusion.SDK.Modules;

namespace EyeTracking.Fusion;

[HarmonyPatch(typeof(EyeTracking.Core))]
public class ModuleLoader
{
    public static void LoadModule()
    {
        ModuleManager.RegisterModule<EyeTrackingModule>();
        var harmony = new HarmonyLib.Harmony("eyetracking.fusion");
        harmony.PatchAll();
    }
    
    [HarmonyPatch(nameof(EyeTracking.Core.OnUpdate))]
    [HarmonyPostfix]
    public static void OnUpdate()
    {
        if (EyeTracking.Core.HasFusion)
        {
            EyeTrackingModule.Update();
        }
    }
}