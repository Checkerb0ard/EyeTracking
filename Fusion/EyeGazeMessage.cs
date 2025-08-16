using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.Network.Serialization;
using LabFusion.Player;
using LabFusion.SDK.Modules;

using UnityEngine;

namespace EyeTracking.Fusion;

public class EyeGazeData : INetSerializable
{
    public int? GetSize() => 4 * 2;

    public float GazeX;
    public float GazeY;

    public void Serialize(INetSerializer serializer)
    {
        serializer.SerializeValue(ref GazeX);
        serializer.SerializeValue(ref GazeY);
    }
}

public class EyeGazeMessage : ModuleMessageHandler
{
    protected override void OnHandleMessage(ReceivedMessage received)
    {
        if (received.Sender == null)
            return;
        
        var playerID = PlayerIDManager.GetPlayerID(received.Sender.Value);
        NetworkPlayerManager.TryGetPlayer(received.Sender.Value, out var player);
        
        if (player == null || !playerID.IsValid)
            return;
        
        if (player.RigRefs?.RigManager == null)
            return;
        
        var data = received.ReadData<EyeGazeData>();
        
        if (player.RigRefs?.RigManager?.animationRig == null)
        {
            Core.Instance.LoggerInstance.Error($"Player {playerID} has no animation rig.");
            return;
        }
        
        var gaze = new Vector2(data.GazeX, data.GazeY);
        player.RigRefs.RigManager.animationRig.eyeGaze = gaze;
        
        Core.Instance.LoggerInstance.Msg($"Received eye gaze data from player {playerID.SmallID}: {gaze}", ConsoleColor.Cyan);
    }
}