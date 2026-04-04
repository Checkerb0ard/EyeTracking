using EyeTracking.MarrowSDK;
using HarmonyLib;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking.Harmony;

[HarmonyPatch(typeof(Avatar))]
internal static class AvatarPatches
{
    [HarmonyPatch(nameof(Avatar.Awake))]
    [HarmonyPostfix]
    private static void Awake(Avatar __instance)
    {
        // really fucking stupid
        switch (__instance.name)
        {
            case "char_Strong(Clone)":
                AddAvatarDescriptor(__instance, "face.eyeBlink_L", "face.eyeBlink_R", "mesh/head");
                break;
            case "char_Fast(Clone)":
                AddAvatarDescriptor(__instance, "face.eyeBlink_L", "face.eyeBlink_R", "Fast_LOD0/Fast_Head_LOD0");
                break;
            case "char_Fur(Clone)":
                AddAvatarDescriptor(__instance, "fur_Head_blendShape.eyeBlink_L", "fur_Head_blendShape.eyeBlink_R", "Fur_LOD0/Fur_Head_LOD0");
                break;
            case "char_Light(Clone)":
                AddAvatarDescriptor(__instance, "eyeBlinkLeft", "eyeBlinkRight", "Face");
                break;
            case "char_jimmy_V5(Clone)":
                AddAvatarDescriptor(__instance, "faceBlendShape.eyeBlink_L", "faceBlendShape.eyeBlink_R", "jimmy_scaledMeters_grp/jimmy_TRI_grp_fNate/jimmy_face_all_grp/jimmy_face_mesh");
                break;
            case "Ford_BW(Clone)":
                AddAvatarDescriptor(__instance, "faceBlends.eyeBlink_L", "faceBlends.eyeBlink_R", "geoGrp/brett_face");
                break;
            default:
                break;
        }
    }
    
    private static void AddAvatarDescriptor(Avatar avatar, string leftEyeBlinkKey, string rightEyeBlinkKey, string skinnedMeshRendererPath)
    {
        if (avatar.GetComponent<AvatarEyeGazeDescriptor>() != null)
            return;
        
        var descriptor = avatar.gameObject.AddComponent<AvatarEyeGazeDescriptor>();
        descriptor.SkinnedMeshRenderer.Set(avatar.transform.Find(skinnedMeshRendererPath).GetComponent<SkinnedMeshRenderer>());
        descriptor.LeftEyeBlinkShapeKey.Set(descriptor.SkinnedMeshRenderer.Get().sharedMesh.GetBlendShapeIndex(leftEyeBlinkKey));
        descriptor.RightEyeBlinkShapeKey.Set(descriptor.SkinnedMeshRenderer.Get().sharedMesh.GetBlendShapeIndex(rightEyeBlinkKey));
    }
}