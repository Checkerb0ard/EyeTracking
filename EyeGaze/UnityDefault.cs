using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.OpenXR;

namespace EyeTracking.EyeGaze;

public class UnityDefault : EyeGazeImplementation
{
    public override string Name => "UnityDefault";
    public override string DeviceId => "unity.xr.default";
    public override bool IsLoaded => _isLoaded;
    
    private InputDevice _eyeDevice;
    private bool _isLoaded = false;

    public override void Initialize()
    {
        var eyegaze = OpenXRSettings.Instance.features.First(x => x.name.StartsWith("EyeGazeInteraction"));
        eyegaze.enabled = true;
        OpenXRSettings.Instance.ApplySettings();
        
        var devices = new Il2CppSystem.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.EyeTracking, devices);

        if (devices.Count > 0)
        {
            _eyeDevice = devices._items[0];
            Core.Instance.LoggerInstance.Msg($"Eye tracking device found: {_eyeDevice.name}");
            _isLoaded = true;
        }
        else
        {
            Core.Instance.LoggerInstance.Warning("No eye tracking capable device found.");
            _isLoaded = false;
            return;
        }

        if (!_eyeDevice.isValid)
        {
            Core.Instance.LoggerInstance.Warning("Eye tracking device is not valid.");
            _isLoaded = false;
        }
    }

    public override void Update()
    {
        if (!_isLoaded)
            return;
        
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
        
        Tracking.Data.Eye.Left.PupilDiameterMm = 4f;
        Tracking.Data.Eye.Right.PupilDiameterMm = 4f;
        
        Tracking.Data.Eye.MinDilation = 0f;
        Tracking.Data.Eye.MaxDilation = 10f;
    }
}