using BoneLib;
using Il2CppSLZ.Marrow;
using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.SDK.Modules;
using LabFusion.Utilities;
using MelonLoader;
using UnityEngine;

namespace EyeTracking.Fusion;

public class EyeTrackingModule : Module
{
    public static EyeTrackingModule Instance; 
    
    public override string Name => "EyeTracking";
    
    public override string Author => "Checkerboard";

    public override Version Version => new(1, 0, 0);

    public override ConsoleColor Color => ConsoleColor.Green;

    protected override void OnModuleRegistered()
    {
        Instance = this;
        ModuleMessageManager.RegisterHandler<EyeGazeMessage>();
    }

    protected override void OnModuleUnregistered()
    {
    }

    internal static void Update()
    {
        if (!NetworkInfo.HasServer)
            return;
        
        EyeSyncer.UpdateAll();
        
        if (!ImplementationManager.CurrentImplementation.IsLoaded)
            return;
        
        if (Player.RigManager == null)
            return;
        
        var gaze = Tracking.Data.Eye.Combined().Gaze;
        var leftOpenness = Tracking.Data.Eye.Left.Openness;
        var rightOpenness = Tracking.Data.Eye.Right.Openness;
        
        if (leftOpenness < 0.05f)
            gaze = Tracking.Data.Eye.Right.Gaze;
        else if (rightOpenness < 0.05f)
            gaze = Tracking.Data.Eye.Left.Gaze;
        
        SendEyeGaze(gaze, leftOpenness, rightOpenness);
    }

    public static void SendEyeGaze(Vector2 gaze, float leftOpenness, float rightOpenness)
    {
        // randomly get an exception here as fusion doesnt set NetworkInfo.HasSever to false in time when leaving a server. so fuck you, try, catch.
        try
        {
            var data = new EyeGazeData
            {
                GazeX = gaze.x,
                GazeY = gaze.y,
                LeftOpenness = leftOpenness,
                RightOpenness = rightOpenness
            };

            MessageRelay.RelayModule<EyeGazeMessage, EyeGazeData>(data, new MessageRoute(RelayType.ToOtherClients, NetworkChannel.Unreliable));
            
        }
        catch (Exception e)
        {

        }
    }
}