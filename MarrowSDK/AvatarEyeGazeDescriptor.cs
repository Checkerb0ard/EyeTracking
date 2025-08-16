using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking.MarrowSDK
{
#if UNITY_EDITOR
    [RequireComponent(typeof(Avatar))]
#endif
    public class AvatarEyeGazeDescriptor : MonoBehaviour
    {
        private Avatar _avatar;
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        
        public string LeftBlinkShapeKey = "LeftEyeGaze";
        public string RightBlinkShapeKey = "RightEyeGaze";

        public void Awake()
        {
            _avatar = GetComponent<Avatar>();
            if (_avatar == null)
            {
                Core.Instance.LoggerInstance.Warning("Eye Gaze Descriptor on a non avatar object?");
            }
        }
    }
}