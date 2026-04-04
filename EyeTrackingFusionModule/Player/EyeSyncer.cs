using EyeTracking.MarrowSDK;
using Il2CppSLZ.Marrow;
using LabFusion.Entities;
using LabFusion.Utilities;
using UnityEngine;

namespace EyeTracking.Fusion.Player;

internal class EyeSyncer
{
    private const float SmoothingSpeed = 35f;

    private readonly NetworkPlayer _player;

    private Vector2 _targetGaze;
    private float _targetLeftOpenness = 1f;
    private float _targetRightOpenness = 1f;

    private Vector4 _currentGaze = new(0f, 0f, 1f, 2f);

    private AvatarEyeGazeDescriptor _descriptor;

    private float _currentLeftBlink;
    private float _currentRightBlink;

    internal EyeSyncer(NetworkPlayer player)
    {
        _player = player;
    }

    internal void SetTarget(Vector2 gaze, float leftOpenness, float rightOpenness)
    {
        _targetGaze = gaze;
        _targetLeftOpenness = leftOpenness;
        _targetRightOpenness = rightOpenness;
    }

    internal void ApplyGaze(AnimationRig rig)
    {
        var target = new Vector4(_targetGaze.x, _targetGaze.y, 1f, 2f);

        _currentGaze = Vector4.Lerp(
            _currentGaze,
            target,
            SmoothingSpeed * TimeReferences.UnscaledDeltaTime
        );

        rig.eyeGaze = _currentGaze;
    }
    
    internal void ApplyBlink(SkinnedMeshRenderer skinnedMesh, AvatarEyeGazeDescriptor descriptor)
    {
        if (skinnedMesh == null)
            return;

        skinnedMesh.SetBlendShapeWeight(
            descriptor.LeftEyeBlinkShapeKey.Get(),
            _currentLeftBlink * 100f
        );

        skinnedMesh.SetBlendShapeWeight(
            descriptor.RightEyeBlinkShapeKey.Get(),
            _currentRightBlink * 100f
        );
    }

    /// <summary>
    /// Returns false if this syncer should be removed
    /// </summary>
    internal bool Update()
    {
        var rigManager = _player?.RigRefs?.RigManager;
        if (rigManager == null)
            return false;

        var rig = rigManager.animationRig;
        if (rig == null)
            return true;

        RefreshDescriptor(rigManager);
        if (_descriptor == null)
            return true;

        var skinnedMesh = _descriptor.SkinnedMeshRenderer.Get();
        if (skinnedMesh == null)
            return true;

        float delta = SmoothingSpeed * TimeReferences.UnscaledDeltaTime;

        _currentLeftBlink = Mathf.Lerp(_currentLeftBlink, 1f - _targetLeftOpenness, delta);
        _currentRightBlink = Mathf.Lerp(_currentRightBlink, 1f - _targetRightOpenness, delta);

        skinnedMesh.SetBlendShapeWeight(
            _descriptor.LeftEyeBlinkShapeKey.Get(),
            _currentLeftBlink * 100f
        );

        skinnedMesh.SetBlendShapeWeight(
            _descriptor.RightEyeBlinkShapeKey.Get(),
            _currentRightBlink * 100f
        );

        return true;
    }

    private void RefreshDescriptor(RigManager rigManager)
    {
        var avatar = rigManager.avatar;
        if (avatar == null)
            return;

        if (_descriptor != null && _descriptor.gameObject.activeSelf)
            return;

        _descriptor = avatar.gameObject.GetComponent<AvatarEyeGazeDescriptor>();
    }
}