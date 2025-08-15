using UnityEngine;

namespace EyeTracking.MarrowSDK
{
    public class AvatarEyeGazeDescriptor : MonoBehaviour
    {
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        
        public string LeftBlinkShapeKey = "LeftEyeGaze";
        public string RightBlinkShapeKey = "RightEyeGaze";  
    }
}