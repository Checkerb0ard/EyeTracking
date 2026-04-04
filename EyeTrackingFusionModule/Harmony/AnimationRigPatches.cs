using EyeTracking.Fusion.Player;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using LabFusion.Entities;

namespace EyeTracking.Fusion.Harmony;

[HarmonyPatch(typeof(AnimationRig))]
internal static class AnimationRigPatches
{
    [HarmonyPatch(nameof(AnimationRig.OnEarlyUpdate))]
    [HarmonyPostfix]
    private static void OnEarlyUpdate(AnimationRig __instance)
    {
        if (!Validate(__instance, out var syncer))
            return;

        syncer.ApplyGaze(__instance);
    }

    private static bool Validate(AnimationRig rig, out EyeSyncer syncer)
    {
        syncer = null;

        if (rig.manager == BoneLib.Player.RigManager)
            return false;

        if (!NetworkPlayerManager.TryGetPlayer(rig.manager, out var player))
            return false;

        if (!EyeSyncerManager.TryGet(player, out syncer))
            return false;

        return true;
    }
}