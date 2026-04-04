using HarmonyLib;
using LabFusion.SDK.Modules;

namespace EyeTracking.Fusion;

[HarmonyPatch(typeof(EyeTracking.Core))]
public static class Core
{
    public static void LoadModule()
    {
        ModuleManager.RegisterModule<EyeTrackingModule>();
        
        new HarmonyLib.Harmony("eyetracking.fusion").PatchAll();
    }
    
    [HarmonyPatch(nameof(EyeTracking.Core.OnUpdate))]
    [HarmonyPostfix]
    internal static void OnUpdate() => EyeTrackingModule.OnUpdate();
}