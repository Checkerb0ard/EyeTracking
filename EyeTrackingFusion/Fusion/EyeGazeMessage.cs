using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.Network.Serialization;
using LabFusion.Player;
using LabFusion.SDK.Modules;
using MelonLoader;
using UnityEngine;

namespace EyeTracking.Fusion;

public class EyeGazeData : INetSerializable
{
    public int? GetSize() => sizeof(float) * 4;

    public float GazeX;
    public float GazeY;
    public float LeftOpenness;
    public float RightOpenness;

    public void Serialize(INetSerializer serializer)
    {
        serializer.SerializeValue(ref GazeX);
        serializer.SerializeValue(ref GazeY);
        serializer.SerializeValue(ref LeftOpenness);
        serializer.SerializeValue(ref RightOpenness);
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
            return;

        EyeSyncer.SetTargetGaze(player, new Vector2(data.GazeX, data.GazeY), data.LeftOpenness, data.RightOpenness);
    }
}