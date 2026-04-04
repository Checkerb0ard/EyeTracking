using EyeTracking.Fusion.Player;
using EyeTracking.MarrowSDK;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using LabFusion.Entities;
using LabFusion.MonoBehaviours;
using LabFusion.Player;

namespace EyeTracking.Fusion.Harmony;

[HarmonyPatch(typeof(Mirror))]
internal static class MirrorPatches
{
    [HarmonyPatch(nameof(Mirror.WriteTransforms))]
    [HarmonyPostfix]
    private static void WriteTransforms(Mirror __instance)
    {
        if (!Validate(__instance, out var syncer, out var descriptor))
            return;

        var skinnedMesh = descriptor.SkinnedMeshRenderer?.Get();
        if (skinnedMesh == null)
            return;

        syncer.ApplyBlink(skinnedMesh, descriptor);
    }

    private static bool Validate(Mirror mirror, out EyeSyncer syncer, out AvatarEyeGazeDescriptor descriptor)
    {
        syncer = null;
        descriptor = null;

        var identifier = mirror.GetComponent<MirrorIdentifier>();
        if (identifier == null || identifier.id == PlayerIDManager.LocalSmallID)
            return false;

        if (!NetworkPlayerManager.TryGetPlayer(identifier.id, out var player))
            return false;

        if (!EyeSyncerManager.TryGet(player, out syncer))
            return false;

        descriptor = mirror.Reflection.GetComponent<AvatarEyeGazeDescriptor>();
        if (descriptor == null)
            return false;

        return true;
    }
}