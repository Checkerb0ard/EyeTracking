using EyeTracking.MarrowSDK;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using LabFusion.SDK.Modules;

namespace EyeTracking.Fusion;

[HarmonyPatch(typeof(EyeTracking.Core))]
public class ModuleLoader
{
    public static void LoadModule()
    {
        ClassInjector.RegisterTypeInIl2Cpp<EyeSyncHelper>();
        
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