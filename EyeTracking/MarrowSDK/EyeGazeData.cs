using UnityEngine;

#if MELONLOADER
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Il2CppSLZ.Marrow;
using Il2CppUltEvents;
using MelonLoader;
#else
using UltEvents;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EyeTracking.MarrowSDK
{
#if MELONLOADER
    [RegisterTypeInIl2Cpp]
#endif
    public class EyeGazeData : MonoBehaviour
    {
        public Vector2 LeftGaze { get; private set; }
        public float LeftPupilDiameterMm { get; private set; }
        public float LeftOpenness { get; private set; }
        public Vector2 RightGaze { get; private set; }
        public float RightPupilDiameterMm { get; private set; }
        public float RightOpenness { get; private set; }
        public Vector2 CombinedGaze { get; private set; }
        public float CombinedPupilDiameterMm { get; private set; }
        public float CombinedOpenness { get; private set; }
        
#if MELONLOADER
        public Il2CppReferenceField<UltEvent> OnEnableEvent;
        public Il2CppReferenceField<UltEvent> OnBlinkEvent;
#else
        public UltEvent OnEnableEvent;
        public UltEvent OnBlinkEvent;
#endif
        
        private bool isPlayerAvatar = false;
        
        private void OnEnable()
        {
#if MELONLOADER
            isPlayerAvatar = gameObject.transform.root.GetComponent<RigManager>() == BoneLib.Player.RigManager;
            
            OnEnableEvent.Value.Invoke();
#else
            OnEnableEvent?.Invoke();
#endif
        }

        private void Update()
        {
#if MELONLOADER
            LeftGaze = new Vector2(Tracking.EyeData.Left.GazeX, Tracking.EyeData.Left.GazeY);
            LeftPupilDiameterMm = Tracking.EyeData.Left.PupilDiameterMm;
            LeftOpenness = Tracking.EyeData.Left.Openness;
            RightGaze = new Vector2(Tracking.EyeData.Right.GazeX, Tracking.EyeData.Right.GazeY);
            RightPupilDiameterMm = Tracking.EyeData.Right.PupilDiameterMm;
            RightOpenness = Tracking.EyeData.Right.Openness;
            CombinedGaze = new Vector2(Tracking.EyeData.Combined().GazeX, Tracking.EyeData.Combined().GazeY);
            CombinedPupilDiameterMm = Tracking.EyeData.Combined().PupilDiameterMm;
            CombinedOpenness = Tracking.EyeData.Combined().Openness;
            
            if (LeftOpenness < 0.1f && RightOpenness < 0.1f)
            {
                OnBlinkEvent?.Value?.Invoke();
            }
#endif
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(EyeGazeData))]
    public class EyeGazeDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("This component allows you to run an UltEvent on Enable. This is useful if you need to add logic for if the user has the eye tracking mod as this component will only be present if that is the case.", MessageType.Info);
            EditorGUILayout.HelpBox("This component also stores all the eye tracking data for easy use in SDK. Access it via UltEvents, as they are stored as properties. Setting them will do nothing, as they are overridden by the mod.", MessageType.Info);
            DrawDefaultInspector();
        }
    }
#endif
}
