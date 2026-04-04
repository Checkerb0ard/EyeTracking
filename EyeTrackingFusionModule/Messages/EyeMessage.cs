using EyeTracking.Fusion.Player;
using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.Player;
using LabFusion.SDK.Modules;
using UnityEngine;

namespace EyeTracking.Fusion.Messages;

internal class EyeMessage : ModuleMessageHandler
{
    protected override void OnHandleMessage(ReceivedMessage received)
    {
        if (received.Sender == null)
            return;

        var playerID = PlayerIDManager.GetPlayerID(received.Sender.Value);

        if (!playerID.IsValid)
            return;

        if (!NetworkPlayerManager.TryGetPlayer(received.Sender.Value, out var player))
            return;

        if (player.RigRefs?.RigManager?.animationRig == null)
            return;

        var data = received.ReadData<EyeData>();
        EyeSyncerManager.SetTarget(player, new Vector2(data.GazeX, data.GazeY), data.LeftOpenness, data.RightOpenness);
    }
}