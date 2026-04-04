using UnityEngine;

namespace EyeTracking.Utilities;

public static class UnityUtilities
{
    public static Vector2 FlipXCoordinates(this Vector2 vector)
    {
        vector.x *= -1f;
        return vector;
    }
    
    public static Vector2 FlipYCoordinates(this Vector2 vector)
    {
        vector.y *= -1f;
        return vector;
    }
}