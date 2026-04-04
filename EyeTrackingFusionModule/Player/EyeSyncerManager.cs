using LabFusion.Entities;
using UnityEngine;

namespace EyeTracking.Fusion.Player;

internal static class EyeSyncerManager
{
    private static readonly Dictionary<NetworkPlayer, EyeSyncer> _syncers = new();

    internal static EyeSyncer GetOrCreate(NetworkPlayer player)
    {
        if (!_syncers.TryGetValue(player, out var syncer))
        {
            syncer = new EyeSyncer(player);
            _syncers[player] = syncer;
        }

        return syncer;
    }

    internal static bool TryGet(NetworkPlayer player, out EyeSyncer syncer)
    {
        return _syncers.TryGetValue(player, out syncer);
    }

    internal static void Remove(NetworkPlayer player)
    {
        _syncers.Remove(player);
    }

    internal static void UpdateAll()
    {
        foreach (var (player, syncer) in _syncers.ToArray())
        {
            if (!syncer.Update())
                _syncers.Remove(player);
        }
    }

    internal static void SetTarget(
        NetworkPlayer player,
        Vector2 gaze,
        float leftOpenness,
        float rightOpenness)
    {
        GetOrCreate(player).SetTarget(gaze, leftOpenness, rightOpenness);
    }
}