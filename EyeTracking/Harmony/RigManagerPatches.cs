using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.VRMK;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(RigManager))]
internal static class RigManagerPatches
{
    [HarmonyPatch(typeof(RigManager), nameof(RigManager.SwitchAvatar))]
    [HarmonyPostfix]
    private static void SwitchAvatar(RigManager __instance, Avatar newAvatar)
    {
        if (!Validate(__instance, newAvatar))
            return;
        
        Core.Instance.SolverManager.EyeSolver.OnAvatarSwitch(newAvatar);
        Core.Instance.SolverManager.FaceSolver.OnAvatarSwitch(newAvatar);
    }

    private static bool Validate(RigManager rigManager, Avatar avatar)
    {
        if (!Core.Instance.PreferencesManager.Enabled.Value)
            return false;
        
        if (!Tracking.IsTracking)
            return false;
        
        if (Player.RigManager != null && rigManager != Player.RigManager)
            return false;
        
        if (avatar == null)
            return false;
        
        return true;
    }
}