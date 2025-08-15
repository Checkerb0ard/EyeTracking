using UnityEngine;
using UnityEngine.XR;

namespace EyeTracking.EyeGaze;

public class UnityDefault : EyeGazeImplementation
{
    public override string Name => "UnityDefault";
    public override string DeviceId => "unity.xr.default";
    private InputDevice _eyeDevice;
    
    // guess work
    private static readonly InputFeatureUsage<float> LeftPupilDiameter = new InputFeatureUsage<float>("LeftPupilDiameter");
    private static readonly InputFeatureUsage<float> RightPupilDiameter = new InputFeatureUsage<float>("RightPupilDiameter");

    public override void Initialize()
    {
        var devices = new Il2CppSystem.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, devices);

        if (devices.Count > 0)
        {
            _eyeDevice = devices._items[0];
            Core.Instance.LoggerInstance.Msg($"Eye tracking device found: {_eyeDevice.name}");
        }
        else
        {
            Core.Instance.LoggerInstance.Warning("No eye tracking capable device found.");
            return;
        }

        if (!_eyeDevice.isValid)
        {
            Core.Instance.LoggerInstance.Warning("Eye tracking device is not valid.");
        }
    }

    public override void Update()
    {
        if (!_eyeDevice.isValid)
            return;
        
        if (!_eyeDevice.TryGetFeatureValue(CommonUsages.eyesData, out Eyes eyes))
            return;

        eyes.TryGetLeftEyeOpenAmount(out var leftOpen);
        eyes.TryGetRightEyeOpenAmount(out var rightOpen);

        eyes.TryGetLeftEyeRotation(out var leftRot);
        eyes.TryGetRightEyeRotation(out var rightRot);

        Vector3 leftForward = leftRot * Vector3.forward;
        Vector3 rightForward = rightRot * Vector3.forward;
        
        Tracking.Data.Eye.Left.Gaze = new Vector2(leftForward.x, leftForward.y);
        Tracking.Data.Eye.Right.Gaze = new Vector2(rightForward.x, rightForward.y);

        Tracking.Data.Eye.Left.Openness = leftOpen;
        Tracking.Data.Eye.Right.Openness = rightOpen;

        // has to be supplied by the vendor, Unity does not provide pupil diameter by default. the keys we are using here are common but not guaranteed to be present.
        if (_eyeDevice.TryGetFeatureValue(LeftPupilDiameter, out float leftPupil))
            Tracking.Data.Eye.Left.PupilDiameterMm = leftPupil;

        if (_eyeDevice.TryGetFeatureValue(RightPupilDiameter, out float rightPupil))
            Tracking.Data.Eye.Right.PupilDiameterMm = rightPupil;

        // if no vendor data, set a default value
        if (Tracking.Data.Eye.Left.PupilDiameterMm <= 0f && Tracking.Data.Eye.Right.PupilDiameterMm <= 0f)
        {
            Tracking.Data.Eye.Left.PupilDiameterMm = 4f;
            Tracking.Data.Eye.Right.PupilDiameterMm = 4f;
        }
    }
}