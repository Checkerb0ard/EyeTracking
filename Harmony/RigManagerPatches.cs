using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(RigManager))]
public class RigManagerPatches
{
    [HarmonyPatch(typeof(RigManager), nameof(RigManager.SwapAvatar))]
    [HarmonyPostfix]
    public static void SwapAvatar(RigManager __instance, Il2CppSLZ.VRMK.Avatar newAvatar)
    {
        if (!Core.EnableEyeTracking.Value)
            return;
        
        if (ImplementationManager.CurrentImplementation != null && !ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        if (Player.RigManager != null && __instance != Player.RigManager)
            return;
        
        if (newAvatar == null)
            return;
        
        EyeSolver.OnAvatarSwap(newAvatar);
    }
}