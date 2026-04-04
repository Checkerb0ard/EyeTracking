using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
#if MELONLOADER
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MelonLoader;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EyeTracking.MarrowSDK
{
#if MELONLOADER
    [RegisterTypeInIl2Cpp]
#endif
    public class AvatarFaceDescriptor : MonoBehaviour
    {
#if MELONLOADER
        public Il2CppReferenceField<SkinnedMeshRenderer> skinnedMeshRenderer;
#else
        public SkinnedMeshRenderer skinnedMeshRenderer;
#endif
        
#if MELONLOADER
        public Il2CppValueField<int> EyeLidRight;
        public Il2CppValueField<int> EyeLidLeft;
        public Il2CppValueField<int> EyeLid;
        public Il2CppValueField<int> EyeSquintRight;
        public Il2CppValueField<int> EyeSquintLeft;
        public Il2CppValueField<int> EyeSquint;

        public Il2CppValueField<int> BrowPinchRight;
        public Il2CppValueField<int> BrowPinchLeft;
        public Il2CppValueField<int> BrowLowererRight;
        public Il2CppValueField<int> BrowLowererLeft;
        public Il2CppValueField<int> BrowInnerUpRight;
        public Il2CppValueField<int> BrowInnerUpLeft;
        public Il2CppValueField<int> BrowOuterUpRight;
        public Il2CppValueField<int> BrowOuterUpLeft;

        public Il2CppValueField<int> NoseSneerRight;
        public Il2CppValueField<int> NoseSneerLeft;
        public Il2CppValueField<int> NasalDilationRight;
        public Il2CppValueField<int> NasalDilationLeft;
        public Il2CppValueField<int> NasalConstrictRight;
        public Il2CppValueField<int> NasalConstrictLeft;

        public Il2CppValueField<int> CheekSquintRight;
        public Il2CppValueField<int> CheekSquintLeft;
        public Il2CppValueField<int> CheekPuffSuckRight_Pos;
        public Il2CppValueField<int> CheekPuffSuckRight_Neg;
        public Il2CppValueField<int> CheekPuffSuckLeft_Pos;
        public Il2CppValueField<int> CheekPuffSuckLeft_Neg;

        public Il2CppValueField<int> JawOpen;
        public Il2CppValueField<int> MouthClosed;
        public Il2CppValueField<int> JawX_Pos;
        public Il2CppValueField<int> JawX_Neg;
        public Il2CppValueField<int> JawZ_Pos;
        public Il2CppValueField<int> JawZ_Neg;
        public Il2CppValueField<int> JawClench;
        public Il2CppValueField<int> JawMandibleRaise;

        public Il2CppValueField<int> LipSuckUpperRight;
        public Il2CppValueField<int> LipSuckUpperLeft;
        public Il2CppValueField<int> LipSuckLowerRight;
        public Il2CppValueField<int> LipSuckLowerLeft;
        public Il2CppValueField<int> LipSuckCornerRight;
        public Il2CppValueField<int> LipSuckCornerLeft;

        public Il2CppValueField<int> LipFunnelUpperRight;
        public Il2CppValueField<int> LipFunnelUpperLeft;
        public Il2CppValueField<int> LipFunnelLowerRight;
        public Il2CppValueField<int> LipFunnelLowerLeft;

        public Il2CppValueField<int> LipPuckerUpperRight;
        public Il2CppValueField<int> LipPuckerUpperLeft;
        public Il2CppValueField<int> LipPuckerLowerRight;
        public Il2CppValueField<int> LipPuckerLowerLeft;

        public Il2CppValueField<int> MouthUpperUpRight;
        public Il2CppValueField<int> MouthUpperUpLeft;
        public Il2CppValueField<int> MouthLowerDownRight;
        public Il2CppValueField<int> MouthLowerDownLeft;
        public Il2CppValueField<int> MouthUpperDeepenRight;
        public Il2CppValueField<int> MouthUpperDeepenLeft;
        public Il2CppValueField<int> MouthUpperX_Pos;
        public Il2CppValueField<int> MouthUpperX_Neg;
        public Il2CppValueField<int> MouthLowerX_Pos;
        public Il2CppValueField<int> MouthLowerX_Neg;
        public Il2CppValueField<int> MouthCornerPullRight;
        public Il2CppValueField<int> MouthCornerPullLeft;
        public Il2CppValueField<int> MouthCornerSlantRight;
        public Il2CppValueField<int> MouthCornerSlantLeft;
        public Il2CppValueField<int> MouthDimpleRight;
        public Il2CppValueField<int> MouthDimpleLeft;
        public Il2CppValueField<int> MouthFrownRight;
        public Il2CppValueField<int> MouthFrownLeft;
        public Il2CppValueField<int> MouthStretchRight;
        public Il2CppValueField<int> MouthStretchLeft;
        public Il2CppValueField<int> MouthRaiserUpper;
        public Il2CppValueField<int> MouthRaiserLower;
        public Il2CppValueField<int> MouthPressRight;
        public Il2CppValueField<int> MouthPressLeft;
        public Il2CppValueField<int> MouthTightenerRight;
        public Il2CppValueField<int> MouthTightenerLeft;

        public Il2CppValueField<int> TongueOut;
        public Il2CppValueField<int> TongueX_Pos;
        public Il2CppValueField<int> TongueX_Neg;
        public Il2CppValueField<int> TongueY_Pos;
        public Il2CppValueField<int> TongueY_Neg;
        public Il2CppValueField<int> TongueRoll;
        public Il2CppValueField<int> TongueArchY_Pos;
        public Il2CppValueField<int> TongueArchY_Neg;
        public Il2CppValueField<int> TongueShape_Pos;
        public Il2CppValueField<int> TongueShape_Neg;
        public Il2CppValueField<int> TongueTwistRight;
        public Il2CppValueField<int> TongueTwistLeft;

        public Il2CppValueField<int> SoftPalateClose;
        public Il2CppValueField<int> ThroatSwallow;
        public Il2CppValueField<int> NeckFlexRight;
        public Il2CppValueField<int> NeckFlexLeft;
        
        public Il2CppValueField<int> BrowDownRight;
        public Il2CppValueField<int> BrowDownLeft;
        public Il2CppValueField<int> BrowOuterUp;
        public Il2CppValueField<int> BrowInnerUp;
        public Il2CppValueField<int> BrowUp;
        public Il2CppValueField<int> BrowExpressionRight_Pos; 
        public Il2CppValueField<int> BrowExpressionRight_Neg; 
        public Il2CppValueField<int> BrowExpressionLeft_Pos;  
        public Il2CppValueField<int> BrowExpressionLeft_Neg;  
        public Il2CppValueField<int> BrowExpression_Pos;      
        public Il2CppValueField<int> BrowExpression_Neg;      
        
        public Il2CppValueField<int> MouthX_Pos; 
        public Il2CppValueField<int> MouthX_Neg; 
        public Il2CppValueField<int> MouthUpperUp;
        public Il2CppValueField<int> MouthLowerDown;
        public Il2CppValueField<int> MouthOpen;
        public Il2CppValueField<int> MouthSmileRight;
        public Il2CppValueField<int> MouthSmileLeft;
        public Il2CppValueField<int> MouthSadRight;
        public Il2CppValueField<int> MouthSadLeft;
        public Il2CppValueField<int> SmileFrownRight_Pos; 
        public Il2CppValueField<int> SmileFrownRight_Neg; 
        public Il2CppValueField<int> SmileFrownLeft_Pos;  
        public Il2CppValueField<int> SmileFrownLeft_Neg;  
        public Il2CppValueField<int> SmileFrown_Pos;      
        public Il2CppValueField<int> SmileFrown_Neg;      
        public Il2CppValueField<int> SmileSadRight_Pos;   
        public Il2CppValueField<int> SmileSadRight_Neg;   
        public Il2CppValueField<int> SmileSadLeft_Pos;    
        public Il2CppValueField<int> SmileSadLeft_Neg;    
        public Il2CppValueField<int> SmileSad_Pos;        
        public Il2CppValueField<int> SmileSad_Neg;        
        
        public Il2CppValueField<int> LipSuckUpper;
        public Il2CppValueField<int> LipSuckLower;
        public Il2CppValueField<int> LipSuck;
        public Il2CppValueField<int> LipFunnelUpper;
        public Il2CppValueField<int> LipFunnelLower;
        public Il2CppValueField<int> LipFunnel;
        public Il2CppValueField<int> LipPuckerUpper;
        public Il2CppValueField<int> LipPuckerLower;
        public Il2CppValueField<int> LipPucker;
        
        public Il2CppValueField<int> NoseSneer;
        public Il2CppValueField<int> CheekSquint;
        public Il2CppValueField<int> CheekPuffSuck_Pos;
        public Il2CppValueField<int> CheekPuffSuck_Neg;
#else
        public int EyeLidRight = -1;
        public int EyeLidLeft = -1;
        public int EyeLid = -1;
        public int EyeSquintRight = -1;
        public int EyeSquintLeft = -1;
        public int EyeSquint = -1;

        public int BrowPinchRight = -1;
        public int BrowPinchLeft = -1;
        public int BrowLowererRight = -1;
        public int BrowLowererLeft = -1;
        public int BrowInnerUpRight = -1;
        public int BrowInnerUpLeft = -1;
        public int BrowOuterUpRight = -1;
        public int BrowOuterUpLeft = -1;

        public int NoseSneerRight = -1;
        public int NoseSneerLeft = -1;
        public int NasalDilationRight = -1;
        public int NasalDilationLeft = -1;
        public int NasalConstrictRight = -1;
        public int NasalConstrictLeft = -1;

        public int CheekSquintRight = -1;
        public int CheekSquintLeft = -1;
        public int CheekPuffSuckRight_Pos = -1;
        public int CheekPuffSuckRight_Neg = -1;
        public int CheekPuffSuckLeft_Pos = -1;
        public int CheekPuffSuckLeft_Neg = -1;

        public int JawOpen = -1;
        public int MouthClosed = -1;
        public int JawX_Pos = -1;
        public int JawX_Neg = -1;
        public int JawZ_Pos = -1;
        public int JawZ_Neg = -1;
        public int JawClench = -1;
        public int JawMandibleRaise = -1;

        public int LipSuckUpperRight = -1;
        public int LipSuckUpperLeft = -1;
        public int LipSuckLowerRight = -1;
        public int LipSuckLowerLeft = -1;
        public int LipSuckCornerRight = -1;
        public int LipSuckCornerLeft = -1;

        public int LipFunnelUpperRight = -1;
        public int LipFunnelUpperLeft = -1;
        public int LipFunnelLowerRight = -1;
        public int LipFunnelLowerLeft = -1;

        public int LipPuckerUpperRight = -1;
        public int LipPuckerUpperLeft = -1;
        public int LipPuckerLowerRight = -1;
        public int LipPuckerLowerLeft = -1;

        public int MouthUpperUpRight = -1;
        public int MouthUpperUpLeft = -1;
        public int MouthLowerDownRight = -1;
        public int MouthLowerDownLeft = -1;
        public int MouthUpperDeepenRight = -1;
        public int MouthUpperDeepenLeft = -1;
        public int MouthUpperX_Pos = -1;
        public int MouthUpperX_Neg = -1;
        public int MouthLowerX_Pos = -1;
        public int MouthLowerX_Neg = -1;
        public int MouthCornerPullRight = -1;
        public int MouthCornerPullLeft = -1;
        public int MouthCornerSlantRight = -1;
        public int MouthCornerSlantLeft = -1;
        public int MouthDimpleRight = -1;
        public int MouthDimpleLeft = -1;
        public int MouthFrownRight = -1;
        public int MouthFrownLeft = -1;
        public int MouthStretchRight = -1;
        public int MouthStretchLeft = -1;
        public int MouthRaiserUpper = -1;
        public int MouthRaiserLower = -1;
        public int MouthPressRight = -1;
        public int MouthPressLeft = -1;
        public int MouthTightenerRight = -1;
        public int MouthTightenerLeft = -1;

        public int TongueOut = -1;
        public int TongueX_Pos = -1;
        public int TongueX_Neg = -1;
        public int TongueY_Pos = -1;
        public int TongueY_Neg = -1;
        public int TongueRoll = -1;
        public int TongueArchY_Pos = -1;
        public int TongueArchY_Neg = -1;
        public int TongueShape_Pos = -1;
        public int TongueShape_Neg = -1;
        public int TongueTwistRight = -1;
        public int TongueTwistLeft = -1;

        public int SoftPalateClose = -1;
        public int ThroatSwallow = -1;
        public int NeckFlexRight = -1;
        public int NeckFlexLeft = -1;

        public int BrowDownRight = -1;
        public int BrowDownLeft = -1;
        public int BrowOuterUp = -1;
        public int BrowInnerUp = -1;
        public int BrowUp = -1;
        public int BrowExpressionRight_Pos = -1;
        public int BrowExpressionRight_Neg = -1;
        public int BrowExpressionLeft_Pos = -1;
        public int BrowExpressionLeft_Neg = -1;
        public int BrowExpression_Pos = -1;
        public int BrowExpression_Neg = -1;

        public int MouthX_Pos = -1;
        public int MouthX_Neg = -1;
        public int MouthUpperUp = -1;
        public int MouthLowerDown = -1;
        public int MouthOpen = -1;
        public int MouthSmileRight = -1;
        public int MouthSmileLeft = -1;
        public int MouthSadRight = -1;
        public int MouthSadLeft = -1;
        public int SmileFrownRight_Pos = -1;
        public int SmileFrownRight_Neg = -1;
        public int SmileFrownLeft_Pos = -1;
        public int SmileFrownLeft_Neg = -1;
        public int SmileFrown_Pos = -1;
        public int SmileFrown_Neg = -1;
        public int SmileSadRight_Pos = -1;
        public int SmileSadRight_Neg = -1;
        public int SmileSadLeft_Pos = -1;
        public int SmileSadLeft_Neg = -1;
        public int SmileSad_Pos = -1;
        public int SmileSad_Neg = -1;

        public int LipSuckUpper = -1;
        public int LipSuckLower = -1;
        public int LipSuck = -1;
        public int LipFunnelUpper = -1;
        public int LipFunnelLower = -1;
        public int LipFunnel = -1;
        public int LipPuckerUpper = -1;
        public int LipPuckerLower = -1;
        public int LipPucker = -1;

        public int NoseSneer = -1;
        public int CheekSquint = -1;
        public int CheekPuffSuck_Pos = -1;
        public int CheekPuffSuck_Neg = -1;
#endif

        private void Update()
        {
#if MELONLOADER
            if (Core.Instance.SolverManager.FaceSolver.CurrentDescriptor == this)
                return;
            
            /*
            if (Core.EnableDebugFaceSolver)
            {
                FaceSolver.SetFaceWeight(this);
            }
            */
#endif
        }

        public void SetMapping(string fieldName, int blendShapeIndex)
        {
#if MELONLOADER
            var field = GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
            if (field != null && field.FieldType == typeof(Il2CppValueField<int>))
            {
                var il2cppField = (Il2CppValueField<int>)field.GetValue(this);
                il2cppField.Value = blendShapeIndex;
            }
#endif
        }

        public void SetMapping(string fieldName, string blendShapeName)
        {
#if MELONLOADER
            if (skinnedMeshRenderer.Value == null || skinnedMeshRenderer.Value.sharedMesh == null)
                return;

            int blendShapeIndex = skinnedMeshRenderer.Value.sharedMesh.GetBlendShapeIndex(blendShapeName);
            if (blendShapeIndex == -1)
            {
                Core.Instance.LoggerInstance.Warning($"Blend shape '{blendShapeName}' not found in the mesh.");
                return;
            }

            SetMapping(fieldName, blendShapeIndex);
#endif
        }
        
#if UNITY_EDITOR
        [ContextMenu("Swap 0 to -1")]
        private void UnmapAllFields()
        {
            var intFields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(int));

            foreach (var field in intFields)
            {
                if ((int)field.GetValue(this) == 0)
                {
                    field.SetValue(this, -1);
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AvatarFaceDescriptor))]
    public class AvatarFaceDescriptorEditor : Editor
    {
        private AvatarFaceDescriptor _descriptor;
        private SkinnedMeshRenderer _renderer;
        private string[] _blendShapeNames;
        private int _selectedBlendShape;
        private string[] _intFieldNames;
        private int _selectedField;
        private bool _showMappingPopup;

        private string _pasteStatus;
        private MessageType _pasteStatusType = MessageType.Info;

        [Serializable]
        private class BlendShapeMappingSet
        {
            public List<BlendShapeMappingEntry> items = new List<BlendShapeMappingEntry>();
        }

        [Serializable]
        private class BlendShapeMappingEntry
        {
            public string field;
            public int index;
        }

        private void OnEnable()
        {
            _descriptor = (AvatarFaceDescriptor)target;
            _renderer = _descriptor.skinnedMeshRenderer;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("skinnedMeshRenderer"));
            _renderer = _descriptor.skinnedMeshRenderer;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Mapped Blend Shapes", EditorStyles.boldLabel);

            var intFields = typeof(AvatarFaceDescriptor)
                .GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => f.FieldType == typeof(int))
                .ToArray();

            if (_renderer != null && _renderer.sharedMesh != null)
            {
                int blendShapeCount = _renderer.sharedMesh.blendShapeCount;
                _blendShapeNames = new string[blendShapeCount];
                for (int i = 0; i < blendShapeCount; i++)
                    _blendShapeNames[i] = _renderer.sharedMesh.GetBlendShapeName(i);
            }

            bool hasMappings = false;
            foreach (var field in intFields)
            {
                int value = (int)field.GetValue(_descriptor);
                if (value > 0 && _blendShapeNames != null && value < _blendShapeNames.Length)
                {
                    hasMappings = true;
                    string mappingInfo = $"BlendShape: {_blendShapeNames[value]} (Index: {value})";
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(field.Name, GUILayout.Width(180));
                    EditorGUILayout.LabelField(mappingInfo, GUILayout.Width(220));
                    if (GUILayout.Button("Unmap", GUILayout.Width(60)))
                    {
                        Undo.RecordObject(_descriptor, $"Unmap {field.Name}");
                        field.SetValue(_descriptor, 0);
                        EditorUtility.SetDirty(_descriptor);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            if (!hasMappings)
            {
                EditorGUILayout.HelpBox("No blend shape mappings created.", MessageType.Info);
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Copy Mapping", GUILayout.Height(22)))
            {
                CopyMapping(intFields);
            }
            if (GUILayout.Button("Paste Mapping", GUILayout.Height(22)))
            {
                PasteMapping(intFields);
            }
            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(_pasteStatus))
            {
                EditorGUILayout.HelpBox(_pasteStatus, _pasteStatusType);
            }

            EditorGUILayout.Space();

            if (_renderer == null)
            {
                EditorGUILayout.HelpBox("Please assign a SkinnedMeshRenderer.", MessageType.Warning);
            }
            else
            {
                if (GUILayout.Button("Create Mapping"))
                {
                    int blendShapeCount = _renderer.sharedMesh != null ? _renderer.sharedMesh.blendShapeCount : 0;
                    _blendShapeNames = new string[blendShapeCount];
                    for (int i = 0; i < blendShapeCount; i++)
                        _blendShapeNames[i] = _renderer.sharedMesh.GetBlendShapeName(i);

                    _intFieldNames = intFields.Select(f => f.Name).ToArray();
                    _selectedBlendShape = 0;
                    _selectedField = 0;
                    _showMappingPopup = true;
                }
            }

            if (_showMappingPopup)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Blend Shape Mapping", EditorStyles.boldLabel);

                if (_blendShapeNames == null || _blendShapeNames.Length == 0)
                {
                    EditorGUILayout.HelpBox("No blend shapes found.", MessageType.Error);
                }
                else if (_intFieldNames == null || _intFieldNames.Length == 0)
                {
                    EditorGUILayout.HelpBox("No int fields found.", MessageType.Error);
                }
                else
                {
                    _selectedBlendShape = EditorGUILayout.Popup("Blend Shape", _selectedBlendShape, _blendShapeNames);
                    _selectedField = EditorGUILayout.Popup("Target Field", _selectedField, _intFieldNames);

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Map"))
                    {
                        Undo.RecordObject(_descriptor, "Map BlendShape to Field");
                        var field = typeof(AvatarFaceDescriptor).GetField(_intFieldNames[_selectedField]);
                        field.SetValue(_descriptor, _selectedBlendShape);
                        EditorUtility.SetDirty(_descriptor);
                        _showMappingPopup = false;
                    }
                    if (GUILayout.Button("Cancel"))
                    {
                        _showMappingPopup = false;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void CopyMapping(FieldInfo[] intFields)
        {
            var set = new BlendShapeMappingSet();
            foreach (var f in intFields)
            {
                set.items.Add(new BlendShapeMappingEntry
                {
                    field = f.Name,
                    index = (int)f.GetValue(_descriptor)
                });
            }
            string json = JsonUtility.ToJson(set, true);
            EditorGUIUtility.systemCopyBuffer = json;
            _pasteStatus = "Mapping copied to clipboard.";
            _pasteStatusType = MessageType.Info;
        }

        private void PasteMapping(FieldInfo[] intFields)
        {
            try
            {
                string json = EditorGUIUtility.systemCopyBuffer;
                if (string.IsNullOrWhiteSpace(json))
                {
                    _pasteStatus = "Clipboard empty.";
                    _pasteStatusType = MessageType.Warning;
                    return;
                }

                var set = JsonUtility.FromJson<BlendShapeMappingSet>(json);
                if (set == null || set.items == null || set.items.Count == 0)
                {
                    _pasteStatus = "Invalid mapping JSON.";
                    _pasteStatusType = MessageType.Error;
                    return;
                }

                var fieldLookup = intFields.ToDictionary(f => f.Name, f => f);
                Undo.RecordObject(_descriptor, "Paste BlendShape Mapping");

                int applied = 0;
                foreach (var entry in set.items)
                {
                    if (fieldLookup.TryGetValue(entry.field, out var fi))
                    {
                        fi.SetValue(_descriptor, entry.index);
                        applied++;
                    }
                }

                EditorUtility.SetDirty(_descriptor);
                _pasteStatus = $"Applied {applied} mappings.";
                _pasteStatusType = MessageType.Info;
            }
            catch (Exception ex)
            {
                _pasteStatus = "Paste failed: " + ex.Message;
                _pasteStatusType = MessageType.Error;
            }
        }
    }
#endif
}