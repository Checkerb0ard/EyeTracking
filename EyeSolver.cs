using UnityEngine;

namespace EyeTracking;

public static class EyeSolver
{
    private static readonly Dictionary<int, Quaternion> _neutral = new();
    private static readonly Dictionary<int, Quaternion> _current = new();
    
    // Constants for gaze solving
    private const float MaxYawDeg = 35f; // Horizontal range
    private const float MaxPitchDeg = 25f; // Vertical range
    private const float HorizontalMultiplier = 1.0f;
    private const float VerticalMultiplier = 4.0f;
    private const float SmoothTime = 0.05f; // Seconds
    
    public static void ApplyEye(Transform eye, Vector2 gaze)
    {
        float dt = Time.deltaTime;
        
        int id = eye.GetInstanceID();
        
        if (!_neutral.ContainsKey(id))
        {
            _neutral[id] = eye.localRotation;
            _current[id] = eye.localRotation;
        }

        gaze.x = Mathf.Clamp(gaze.x * HorizontalMultiplier, -1f, 1f);
        gaze.y = Mathf.Clamp(gaze.y * VerticalMultiplier, -1f, 1f);
        
        float yaw = gaze.x * MaxYawDeg;
        float pitch = -gaze.y * MaxPitchDeg;
        
        Quaternion target = _neutral[id] * Quaternion.Euler(pitch, yaw, 0f);
        
        Quaternion next = Smooth(_current[id], target, SmoothTime, dt);

        _current[id] = next;
        eye.localRotation = next;
    }

    public static void ApplyEyeSimple(Transform eye, Vector2 gaze)
    {
        int id = eye.GetInstanceID();
    
        if (!_neutral.ContainsKey(id))
        {
            _neutral[id] = eye.localRotation;
        }
    
        gaze.x = Mathf.Clamp(gaze.x * HorizontalMultiplier, -1f, 1f);
        gaze.y = Mathf.Clamp(gaze.y * VerticalMultiplier, -1f, 1f);
    
        float yaw = gaze.x * MaxYawDeg;
        float pitch = -gaze.y * MaxPitchDeg;
    
        eye.localRotation = _neutral[id] * Quaternion.Euler(pitch, yaw, 0f);
    }
    
    private static Quaternion Smooth(Quaternion current, Quaternion target, float smoothTime, float dt)
    {
        if (smoothTime <= 0f) return target;
        float t = 1f - Mathf.Exp(-dt / Mathf.Max(0.0001f, smoothTime));
        return Quaternion.Slerp(current, target, t);
    }
}