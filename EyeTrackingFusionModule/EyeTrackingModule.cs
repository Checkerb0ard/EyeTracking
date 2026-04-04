using EyeTracking.Fusion.Messages;
using EyeTracking.Fusion.Player;
using EyeTracking.Fusion.Senders;
using LabFusion.Entities;
using LabFusion.Network;
using LabFusion.SDK.Modules;

namespace EyeTracking.Fusion;

internal class EyeTrackingModule : Module
{
    internal static EyeTrackingModule Instance { get; private set; }
    
    public override string Name => "EyeTracking";
    public override string Author => "Checkerboard";
    public override Version Version => new(1, 0, 0);
    public override ConsoleColor Color => ConsoleColor.Green;
    
    protected override void OnModuleRegistered()
    {
        Instance = this;
        
        ModuleMessageManager.RegisterHandler<EyeMessage>();
    }

    internal static void OnUpdate()
    {
        if (!NetworkInfo.HasServer)
            return;
        
        // Basically a "have we fully loaded check"
        // Fixes the joining bug that was caused by not having a full connection to the server yet and trying to send eye data
        if (!NetworkPlayerManager.TryGetPlayer(0, out var player) || player?.RigRefs == null)
            return;
        
        EyeSyncerManager.UpdateAll();
        
        if (!Tracking.IsTracking)
            return;
        
        if (Tracking.SupportsEye)
            EyeSender.SendCurrent();
        
        if (Tracking.SupportsFace)
            FaceSender.SendCurrent();
    }
}