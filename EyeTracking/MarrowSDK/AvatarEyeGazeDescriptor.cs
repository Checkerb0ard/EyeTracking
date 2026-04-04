#if MELONLOADER
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MelonLoader;
using Avatar = Il2CppSLZ.VRMK.Avatar;
#else
using Avatar = SLZ.VRMK.Avatar;
#endif

using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EyeTracking.MarrowSDK
{
#if MELONLOADER
    [RegisterTypeInIl2Cpp]
#else
    [RequireComponent(typeof(Avatar))]
#endif
    public class AvatarEyeGazeDescriptor : MonoBehaviour
    {
#if MELONLOADER
        public Il2CppReferenceField<SkinnedMeshRenderer> SkinnedMeshRenderer;
        public Il2CppValueField<int> LeftEyeBlinkShapeKey;
        public Il2CppValueField<int> RightEyeBlinkShapeKey;
#else
        [HideInInspector]
        public SkinnedMeshRenderer SkinnedMeshRenderer;
        [HideInInspector]
        public int LeftEyeBlinkShapeKey = -1;
        [HideInInspector]
        public int RightEyeBlinkShapeKey = -1;
#endif

        public void Awake()
        {
#if MELONLOADER
            if (GetComponent<Avatar>() == null)
            {
                Core.Instance.LoggerInstance.Warning("AvatarEyeGazeDescriptor needs to be on an Avatar.");
                return;
            }
#endif
        }

        private void Update()
        {
#if MELONLOADER
            if (Core.Instance.SolverManager.EyeSolver.CurrentDescriptor == this)
                return;
            
            /*
            if (Core.EnableDebugEyeSolver)
            {
                EyeSolver.SetBlinkWeight(this);
            }
            */
#endif
        }

#if UNITY_EDITOR
        [ContextMenu("Copy Renderer Path")]
        private void CopyRendererPath()
        {
            if (SkinnedMeshRenderer == null)
            {
                Debug.LogWarning("No SkinnedMeshRenderer assigned.");
                return;
            }

            Transform root = gameObject.transform;
            Transform current = SkinnedMeshRenderer.transform;
    
            List<string> pathParts = new List<string>();
    
            while (current != null && current != root)
            {
                pathParts.Add(current.name);
                current = current.parent;
            }
    
            pathParts.Reverse();
            string path = string.Join("/", pathParts);
    
            GUIUtility.systemCopyBuffer = path;
        }
#endif
    }
    
#if UNITY_EDITOR

    [CustomEditor(typeof(AvatarEyeGazeDescriptor))]
    public class AvatarEyeGazeDescriptorEditor : Editor
    {
        private bool showWizard = false;
        private int wizardStep = 0;
        private bool showSuccessMessage = false;
        private double successMessageTime = 0;
        private SkinnedMeshRenderer selectedRenderer;
        private string[] blendShapeNames;
        private int selectedLeftBlinkIndex = -1;
        private int selectedRightBlinkIndex = -1;

        public override void OnInspectorGUI()
        {
            AvatarEyeGazeDescriptor descriptor = (AvatarEyeGazeDescriptor)target;

            EditorGUILayout.Space();
            
            if (!showWizard)
            {
                DrawDefaultInspector();
                
                EditorGUILayout.Space();
                
                if (showSuccessMessage)
                {
                    if (EditorApplication.timeSinceStartup < successMessageTime)
                    {
                        EditorGUILayout.HelpBox("Configuration applied successfully!", MessageType.Info);
                        Repaint();
                    }
                    else
                    {
                        showSuccessMessage = false;
                    }
                }

                if (descriptor.GetComponent<Avatar>().eyeCenterOverride != null)
                {
                    EditorGUILayout.HelpBox("Avatar is using an eye center override. Eye tracking will NOT work!", MessageType.Error);
                }
                
                if (GUILayout.Button("Start Setup Wizard", GUILayout.Height(30)))
                {
                    showWizard = true;
                    wizardStep = 0;
                    selectedRenderer = descriptor.SkinnedMeshRenderer;
                }
            }
            else
            {
                DrawWizard(descriptor);
            }
        }

        private void DrawWizard(AvatarEyeGazeDescriptor descriptor)
        {
            EditorGUILayout.LabelField("Eye Tracking Setup Wizard", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            switch (wizardStep)
            {
                case 0:
                    DrawSkinnedMeshRendererStep(descriptor);
                    break;
                case 1:
                    DrawBlendShapeSelectionStep(descriptor);
                    break;
                case 2:
                    DrawCompletionStep(descriptor);
                    break;
            }

            EditorGUILayout.Space();
            DrawWizardNavigation();
        }

        private void DrawSkinnedMeshRendererStep(AvatarEyeGazeDescriptor descriptor)
        {
            EditorGUILayout.LabelField("Step 1: Select Skinned Mesh Renderer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Select the SkinnedMeshRenderer that contains the eye blink blend shapes.", MessageType.Info);
            
            selectedRenderer = (SkinnedMeshRenderer)EditorGUILayout.ObjectField(
                "Skinned Mesh Renderer", 
                selectedRenderer, 
                typeof(SkinnedMeshRenderer), 
                true
            );

            if (selectedRenderer != null && selectedRenderer.sharedMesh != null)
            {
                int blendShapeCount = selectedRenderer.sharedMesh.blendShapeCount;
                
                if (blendShapeCount > 0)
                {
                    EditorGUILayout.HelpBox($"Blend Shapes Found: {blendShapeCount} \nClick Next to select blink shapes.", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("No blend shapes found in this mesh.", MessageType.Warning);
                }
            }
        }

        private void DrawBlendShapeSelectionStep(AvatarEyeGazeDescriptor descriptor)
        {
            EditorGUILayout.LabelField("Step 2: Select Blink Blend Shapes", EditorStyles.boldLabel);
            
            if (selectedRenderer?.sharedMesh == null)
            {
                EditorGUILayout.HelpBox("No valid SkinnedMeshRenderer selected.", MessageType.Error);
                return;
            }

            Mesh mesh = selectedRenderer.sharedMesh;
            int blendShapeCount = mesh.blendShapeCount;
            
            if (blendShapeNames == null || blendShapeNames.Length != blendShapeCount)
            {
                blendShapeNames = new string[blendShapeCount];
                for (int i = 0; i < blendShapeCount; i++)
                {
                    blendShapeNames[i] = mesh.GetBlendShapeName(i);
                }
            }

            EditorGUILayout.HelpBox("Select the blend shapes that control left and right eye blinking.", MessageType.Info);
            
            EditorGUILayout.LabelField("Left Eye Blink Shape:");
            selectedLeftBlinkIndex = EditorGUILayout.Popup(selectedLeftBlinkIndex, blendShapeNames);
            
            EditorGUILayout.LabelField("Right Eye Blink Shape:");
            selectedRightBlinkIndex = EditorGUILayout.Popup(selectedRightBlinkIndex, blendShapeNames);
        }

        private void DrawCompletionStep(AvatarEyeGazeDescriptor descriptor)
        {
            EditorGUILayout.LabelField("Step 3: Complete Setup", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Review your settings and click Apply to complete the setup.", MessageType.Info);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configuration Summary:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"Skinned Mesh Renderer: {selectedRenderer?.name ?? "None"}");
            
            if (blendShapeNames != null)
            {
                EditorGUILayout.LabelField($"Left Eye Blink: {(selectedLeftBlinkIndex >= 0 ? blendShapeNames[selectedLeftBlinkIndex] : "None")}");
                EditorGUILayout.LabelField($"Right Eye Blink: {(selectedRightBlinkIndex >= 0 ? blendShapeNames[selectedRightBlinkIndex] : "None")}");
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Configuration", GUILayout.Height(30)))
            {
                ApplyConfiguration(descriptor);
            }
        }

        private void DrawWizardNavigation()
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Cancel"))
            {
                showWizard = false;
                wizardStep = 0;
            }

            GUILayout.FlexibleSpace();

            if (wizardStep > 0 && GUILayout.Button("Previous"))
            {
                wizardStep--;
            }

            bool canProceed = CanProceedToNextStep();
            GUI.enabled = canProceed;
            
            if (wizardStep < 2 && GUILayout.Button("Next"))
            {
                wizardStep++;
            }
            
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }

        private bool CanProceedToNextStep()
        {
            switch (wizardStep)
            {
                case 0:
                    return selectedRenderer != null && selectedRenderer.sharedMesh != null && selectedRenderer.sharedMesh.blendShapeCount > 0;
                case 1:
                    return selectedLeftBlinkIndex >= 0 && selectedRightBlinkIndex >= 0;
                default:
                    return true;
            }
        }

        private void ApplyConfiguration(AvatarEyeGazeDescriptor descriptor)
        {
            Undo.RecordObject(descriptor, "Configure Eye Tracking");
            
            descriptor.SkinnedMeshRenderer = selectedRenderer;
            descriptor.LeftEyeBlinkShapeKey = selectedLeftBlinkIndex;
            descriptor.RightEyeBlinkShapeKey = selectedRightBlinkIndex;
            
            EditorUtility.SetDirty(descriptor);
            
            showWizard = false;
            wizardStep = 0;
            
            showSuccessMessage = true;
            successMessageTime = EditorApplication.timeSinceStartup + 3.0;
            
            Debug.Log("Configuration applied successfully!");
        }
    }
#endif
}