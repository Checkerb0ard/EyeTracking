using UnityEngine;

#if MELONLOADER
using Il2CppInterop.Runtime.InteropTypes.Fields;
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
        
        private void OnEnable()
        {
#if MELONLOADER
            OnEnableEvent?.Value?.Invoke();
#else
            OnEnableEvent?.Invoke();
#endif
        }

        private void Update()
        {
#if MELONLOADER
            LeftGaze = Tracking.Data.Eye.Left.Gaze;
            LeftPupilDiameterMm = Tracking.Data.Eye.Left.PupilDiameterMm;
            LeftOpenness = Tracking.Data.Eye.Left.Openness;
            RightGaze = Tracking.Data.Eye.Right.Gaze;
            RightPupilDiameterMm = Tracking.Data.Eye.Right.PupilDiameterMm;
            RightOpenness = Tracking.Data.Eye.Right.Openness;
            CombinedGaze = Tracking.Data.Eye.Combined().Gaze;
            CombinedPupilDiameterMm = Tracking.Data.Eye.Combined().PupilDiameterMm;
            CombinedOpenness = Tracking.Data.Eye.Combined().Openness;
            
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
