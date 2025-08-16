using BoneLib;

using LabFusion.Network;
using LabFusion.SDK.Modules;

using UnityEngine;

namespace EyeTracking.Fusion;

public class EyeTrackingModule : Module
{
    public override string Name => "EyeTracking Module";
    
    public override string Author => "Checkerboard";

    public override Version Version => new(1, 0, 0);

    public override ConsoleColor Color => ConsoleColor.Green;

    protected override void OnModuleRegistered()
    {
        ModuleMessageManager.RegisterHandler<EyeGazeMessage>();
    }

    protected override void OnModuleUnregistered()
    {
    }

    internal static void Update()
    {
        if (!NetworkInfo.HasServer)
            return;
        
        if (Player.RigManager == null)
            return;
        
        SendEyeGaze(Tracking.Data.Eye.Combined().Gaze);
    }

    public static void SendEyeGaze(Vector2 gaze)
    {
        var data = new EyeGazeData
        {
            GazeX = gaze.x,
            GazeY = gaze.y
        };

        MessageRelay.RelayModule<EyeGazeMessage, EyeGazeData>(data, new MessageRoute(RelayType.ToOtherClients, NetworkChannel.Unreliable));
    }
}