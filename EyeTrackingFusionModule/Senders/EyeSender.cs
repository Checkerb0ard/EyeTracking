using EyeTracking.Fusion.Messages;
using LabFusion.Network;
using UnityEngine;

namespace EyeTracking.Fusion.Senders;

internal static class EyeSender
{
    internal static void SendCurrent()
    {
        var leftOpenness = Tracking.EyeData.Left.Openness;
        var rightOpenness = Tracking.EyeData.Right.Openness;

        var source =
            leftOpenness < 0.05f ? Tracking.EyeData.Right :
            rightOpenness < 0.05f ? Tracking.EyeData.Left :
            Tracking.EyeData.Combined();

        SendEyeGaze(new Vector2(source.GazeX, source.GazeY), leftOpenness, rightOpenness);
    }
    
    private static void SendEyeGaze(Vector2 gaze, float leftOpenness, float rightOpenness)
    {
        try
        {
            var data = new EyeData
            {
                GazeX = gaze.x,
                GazeY = gaze.y,
                LeftOpenness = leftOpenness,
                RightOpenness = rightOpenness,
            };

            MessageRelay.RelayModule<EyeMessage, EyeData>(data, new MessageRoute(RelayType.ToOtherClients, NetworkChannel.Unreliable));
        }
        catch (Exception e)
        {
            EyeTrackingModule.Instance.LoggerInstance.LogException("sending eye gaze message", e);
        }
    }
}