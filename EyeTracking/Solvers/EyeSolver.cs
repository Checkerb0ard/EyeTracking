using EyeTracking.MarrowSDK;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking.Solvers;

internal class EyeSolver : ISolver
{
    internal AvatarEyeGazeDescriptor CurrentDescriptor { get; private set; }
    
    private static readonly float SmoothingSpeed = 35f;
    private static float _smoothedLeftBlink;
    private static float _smoothedRightBlink;
    
    public void OnAvatarSwitch(Avatar avatar)
    {
        if (avatar == null)
            return;
        
        CurrentDescriptor = avatar.GetComponent<AvatarEyeGazeDescriptor>();
    }

    public void Update()
    {
        if (!Core.Instance.PreferencesManager.Enabled.Value)
            return;
        
        if (!Tracking.IsTracking || !Tracking.SupportsEye)
            return;
        
        if (CurrentDescriptor == null)
            return;
        
        SetBlinkWeight(CurrentDescriptor);
    }
        
    internal void SetBlinkWeight(AvatarEyeGazeDescriptor descriptor)
    {
        var smr = descriptor.SkinnedMeshRenderer.Get();
        if (smr == null || descriptor.LeftEyeBlinkShapeKey.Get() == -1 || descriptor.RightEyeBlinkShapeKey.Get() == -1)
            return;
        
        var targetLeftBlink = 1f - Tracking.EyeData.Left.Openness;
        var targetRightBlink = 1f - Tracking.EyeData.Right.Openness;
        
        _smoothedLeftBlink = Mathf.Lerp(_smoothedLeftBlink, Mathf.Clamp(targetLeftBlink, 0f, 1f), SmoothingSpeed * Time.unscaledDeltaTime);
        _smoothedRightBlink = Mathf.Lerp(_smoothedRightBlink, Mathf.Clamp(targetRightBlink, 0f, 1f), SmoothingSpeed * Time.unscaledDeltaTime);
        
        if (float.IsNaN(_smoothedLeftBlink) || float.IsInfinity(_smoothedLeftBlink))
            _smoothedLeftBlink = 0f;
        if (float.IsNaN(_smoothedRightBlink) || float.IsInfinity(_smoothedRightBlink))
            _smoothedRightBlink = 0f;
        
        smr.SetBlendShapeWeight(descriptor.LeftEyeBlinkShapeKey.Get(), _smoothedLeftBlink * 100f);
        smr.SetBlendShapeWeight(descriptor.RightEyeBlinkShapeKey.Get(), _smoothedRightBlink * 100f);
    }
}