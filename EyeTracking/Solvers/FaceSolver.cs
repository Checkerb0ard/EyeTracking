using EyeTracking.MarrowSDK;
using UnityEngine;
using Avatar = Il2CppSLZ.VRMK.Avatar;

namespace EyeTracking.Solvers;

internal class FaceSolver : ISolver
{
    internal AvatarFaceDescriptor CurrentDescriptor { get; private set; }
    
    public void OnAvatarSwitch(Avatar avatar)
    {
        if (avatar == null)
            return;
        
        CurrentDescriptor = avatar.GetComponent<AvatarFaceDescriptor>();
    }

    public void Update()
    {
        if (!Core.Instance.PreferencesManager.Enabled.Value)
            return;
        
        if (!Tracking.IsTracking || !Tracking.SupportsFace)
            return;
        
        if (CurrentDescriptor == null)
            return;
        
        SetFaceWeight(CurrentDescriptor);
    }

    internal void SetFaceWeight(AvatarFaceDescriptor descriptor)
    {
        var smr = descriptor.skinnedMeshRenderer.Get();
        if (descriptor == null || smr == null)
            return;
        
        void Set(int index, float weight, bool negative = false)
        {
            if (index <= -1)
                return;

            float finalWeight = Mathf.Clamp01(negative ? -weight : weight) * 100f;
            smr.SetBlendShapeWeight(index, finalWeight);
        }

        Set(descriptor.EyeLidRight.Value, Tracking.FaceData.EyeLidRight);
        Set(descriptor.EyeLidLeft.Value, Tracking.FaceData.EyeLidLeft);
        Set(descriptor.EyeLid.Value, Tracking.FaceData.EyeLid);
        Set(descriptor.EyeSquintRight.Value, Tracking.FaceData.EyeSquintRight);
        Set(descriptor.EyeSquintLeft.Value, Tracking.FaceData.EyeSquintLeft);
        Set(descriptor.EyeSquint.Value, Tracking.FaceData.EyeSquint);

        Set(descriptor.BrowPinchRight.Value, Tracking.FaceData.BrowPinchRight);
        Set(descriptor.BrowPinchLeft.Value, Tracking.FaceData.BrowPinchLeft);
        Set(descriptor.BrowLowererRight.Value, Tracking.FaceData.BrowLowererRight);
        Set(descriptor.BrowLowererLeft.Value, Tracking.FaceData.BrowLowererLeft);
        Set(descriptor.BrowInnerUpRight.Value, Tracking.FaceData.BrowInnerUpRight);
        Set(descriptor.BrowInnerUpLeft.Value, Tracking.FaceData.BrowInnerUpLeft);
        Set(descriptor.BrowOuterUpRight.Value, Tracking.FaceData.BrowOuterUpRight);
        Set(descriptor.BrowOuterUpLeft.Value, Tracking.FaceData.BrowOuterUpLeft);

        Set(descriptor.NoseSneerRight.Value, Tracking.FaceData.NoseSneerRight);
        Set(descriptor.NoseSneerLeft.Value, Tracking.FaceData.NoseSneerLeft);
        Set(descriptor.NasalDilationRight.Value, Tracking.FaceData.NasalDilationRight);
        Set(descriptor.NasalDilationLeft.Value, Tracking.FaceData.NasalDilationLeft);
        Set(descriptor.NasalConstrictRight.Value, Tracking.FaceData.NasalConstrictRight);
        Set(descriptor.NasalConstrictLeft.Value, Tracking.FaceData.NasalConstrictLeft);

        Set(descriptor.CheekSquintRight.Value, Tracking.FaceData.CheekSquintRight);
        Set(descriptor.CheekSquintLeft.Value, Tracking.FaceData.CheekSquintLeft);
        Set(descriptor.CheekPuffSuckRight_Pos.Value, Tracking.FaceData.CheekPuffSuckRight);
        Set(descriptor.CheekPuffSuckRight_Neg.Value, Tracking.FaceData.CheekPuffSuckRight, true);
        Set(descriptor.CheekPuffSuckLeft_Pos.Value, Tracking.FaceData.CheekPuffSuckLeft);
        Set(descriptor.CheekPuffSuckLeft_Neg.Value, Tracking.FaceData.CheekPuffSuckLeft, true);

        Set(descriptor.JawOpen.Value, Tracking.FaceData.JawOpen);
        Set(descriptor.MouthClosed.Value, Tracking.FaceData.MouthClosed);
        Set(descriptor.JawX_Pos.Value, Tracking.FaceData.JawX);
        Set(descriptor.JawX_Neg.Value, Tracking.FaceData.JawX, true);
        Set(descriptor.JawZ_Pos.Value, Tracking.FaceData.JawZ);
        Set(descriptor.JawZ_Neg.Value, Tracking.FaceData.JawZ, true);
        Set(descriptor.JawClench.Value, Tracking.FaceData.JawClench);
        Set(descriptor.JawMandibleRaise.Value, Tracking.FaceData.JawMandibleRaise);

        Set(descriptor.LipSuckUpperRight.Value, Tracking.FaceData.LipSuckUpperRight);
        Set(descriptor.LipSuckUpperLeft.Value, Tracking.FaceData.LipSuckUpperLeft);
        Set(descriptor.LipSuckLowerRight.Value, Tracking.FaceData.LipSuckLowerRight);
        Set(descriptor.LipSuckLowerLeft.Value, Tracking.FaceData.LipSuckLowerLeft);
        Set(descriptor.LipSuckCornerRight.Value, Tracking.FaceData.LipSuckCornerRight);
        Set(descriptor.LipSuckCornerLeft.Value, Tracking.FaceData.LipSuckCornerLeft);

        Set(descriptor.LipFunnelUpperRight.Value, Tracking.FaceData.LipFunnelUpperRight);
        Set(descriptor.LipFunnelUpperLeft.Value, Tracking.FaceData.LipFunnelUpperLeft);
        Set(descriptor.LipFunnelLowerRight.Value, Tracking.FaceData.LipFunnelLowerRight);
        Set(descriptor.LipFunnelLowerLeft.Value, Tracking.FaceData.LipFunnelLowerLeft);

        Set(descriptor.LipPuckerUpperRight.Value, Tracking.FaceData.LipPuckerUpperRight);
        Set(descriptor.LipPuckerUpperLeft.Value, Tracking.FaceData.LipPuckerUpperLeft);
        Set(descriptor.LipPuckerLowerRight.Value, Tracking.FaceData.LipPuckerLowerRight);
        Set(descriptor.LipPuckerLowerLeft.Value, Tracking.FaceData.LipPuckerLowerLeft);

        Set(descriptor.MouthUpperUpRight.Value, Tracking.FaceData.MouthUpperUpRight);
        Set(descriptor.MouthUpperUpLeft.Value, Tracking.FaceData.MouthUpperUpLeft);
        Set(descriptor.MouthLowerDownRight.Value, Tracking.FaceData.MouthLowerDownRight);
        Set(descriptor.MouthLowerDownLeft.Value, Tracking.FaceData.MouthLowerDownLeft);
        Set(descriptor.MouthUpperDeepenRight.Value, Tracking.FaceData.MouthUpperDeepenRight);
        Set(descriptor.MouthUpperDeepenLeft.Value, Tracking.FaceData.MouthUpperDeepenLeft);
        Set(descriptor.MouthUpperX_Pos.Value, Tracking.FaceData.MouthUpperX);
        Set(descriptor.MouthUpperX_Neg.Value, Tracking.FaceData.MouthUpperX, true);
        Set(descriptor.MouthLowerX_Pos.Value, Tracking.FaceData.MouthLowerX);
        Set(descriptor.MouthLowerX_Neg.Value, Tracking.FaceData.MouthLowerX, true);
        Set(descriptor.MouthCornerPullRight.Value, Tracking.FaceData.MouthCornerPullRight);
        Set(descriptor.MouthCornerPullLeft.Value, Tracking.FaceData.MouthCornerPullLeft);
        Set(descriptor.MouthCornerSlantRight.Value, Tracking.FaceData.MouthCornerSlantRight);
        Set(descriptor.MouthCornerSlantLeft.Value, Tracking.FaceData.MouthCornerSlantLeft);
        Set(descriptor.MouthDimpleRight.Value, Tracking.FaceData.MouthDimpleRight);
        Set(descriptor.MouthDimpleLeft.Value, Tracking.FaceData.MouthDimpleLeft);
        Set(descriptor.MouthFrownRight.Value, Tracking.FaceData.MouthFrownRight);
        Set(descriptor.MouthFrownLeft.Value, Tracking.FaceData.MouthFrownLeft);
        Set(descriptor.MouthStretchRight.Value, Tracking.FaceData.MouthStretchRight);
        Set(descriptor.MouthStretchLeft.Value, Tracking.FaceData.MouthStretchLeft);
        Set(descriptor.MouthRaiserUpper.Value, Tracking.FaceData.MouthRaiserUpper);
        Set(descriptor.MouthRaiserLower.Value, Tracking.FaceData.MouthRaiserLower);
        Set(descriptor.MouthPressRight.Value, Tracking.FaceData.MouthPressRight);
        Set(descriptor.MouthPressLeft.Value, Tracking.FaceData.MouthPressLeft);
        Set(descriptor.MouthTightenerRight.Value, Tracking.FaceData.MouthTightenerRight);
        Set(descriptor.MouthTightenerLeft.Value, Tracking.FaceData.MouthTightenerLeft);

        Set(descriptor.TongueOut.Value, Tracking.FaceData.TongueOut);
        Set(descriptor.TongueX_Pos.Value, Tracking.FaceData.TongueX);
        Set(descriptor.TongueX_Neg.Value, Tracking.FaceData.TongueX, true);
        Set(descriptor.TongueY_Pos.Value, Tracking.FaceData.TongueY);
        Set(descriptor.TongueY_Neg.Value, Tracking.FaceData.TongueY, true);
        Set(descriptor.TongueRoll.Value, Tracking.FaceData.TongueRoll);
        Set(descriptor.TongueArchY_Pos.Value, Tracking.FaceData.TongueArchY);
        Set(descriptor.TongueArchY_Neg.Value, Tracking.FaceData.TongueArchY, true);
        Set(descriptor.TongueShape_Pos.Value, Tracking.FaceData.TongueShape);
        Set(descriptor.TongueShape_Neg.Value, Tracking.FaceData.TongueShape, true);
        Set(descriptor.TongueTwistRight.Value, Tracking.FaceData.TongueTwistRight);
        Set(descriptor.TongueTwistLeft.Value, Tracking.FaceData.TongueTwistLeft);

        Set(descriptor.SoftPalateClose.Value, Tracking.FaceData.SoftPalateClose);
        Set(descriptor.ThroatSwallow.Value, Tracking.FaceData.ThroatSwallow);
        Set(descriptor.NeckFlexRight.Value, Tracking.FaceData.NeckFlexRight);
        Set(descriptor.NeckFlexLeft.Value, Tracking.FaceData.NeckFlexLeft);

        Set(descriptor.BrowDownRight.Value, Tracking.FaceData.BrowDownRight);
        Set(descriptor.BrowDownLeft.Value, Tracking.FaceData.BrowDownLeft);
        Set(descriptor.BrowOuterUp.Value, Tracking.FaceData.BrowOuterUp);
        Set(descriptor.BrowInnerUp.Value, Tracking.FaceData.BrowInnerUp);
        Set(descriptor.BrowUp.Value, Tracking.FaceData.BrowUp);
        Set(descriptor.BrowExpressionRight_Pos.Value, Tracking.FaceData.BrowExpressionRight);
        Set(descriptor.BrowExpressionRight_Neg.Value, Tracking.FaceData.BrowExpressionRight, true);
        Set(descriptor.BrowExpressionLeft_Pos.Value, Tracking.FaceData.BrowExpressionLeft);
        Set(descriptor.BrowExpressionLeft_Neg.Value, Tracking.FaceData.BrowExpressionLeft, true);
        Set(descriptor.BrowExpression_Pos.Value, Tracking.FaceData.BrowExpression);
        Set(descriptor.BrowExpression_Neg.Value, Tracking.FaceData.BrowExpression, true);

        Set(descriptor.MouthX_Pos.Value, Tracking.FaceData.MouthX);
        Set(descriptor.MouthX_Neg.Value, Tracking.FaceData.MouthX, true);
        Set(descriptor.MouthUpperUp.Value, Tracking.FaceData.MouthUpperUp);
        Set(descriptor.MouthLowerDown.Value, Tracking.FaceData.MouthLowerDown);
        Set(descriptor.MouthOpen.Value, Tracking.FaceData.MouthOpen);
        Set(descriptor.MouthSmileRight.Value, Tracking.FaceData.MouthSmileRight);
        Set(descriptor.MouthSmileLeft.Value, Tracking.FaceData.MouthSmileLeft);
        Set(descriptor.MouthSadRight.Value, Tracking.FaceData.MouthSadRight);
        Set(descriptor.MouthSadLeft.Value, Tracking.FaceData.MouthSadLeft);
        Set(descriptor.SmileFrownRight_Pos.Value, Tracking.FaceData.SmileFrownRight);
        Set(descriptor.SmileFrownRight_Neg.Value, Tracking.FaceData.SmileFrownRight, true);
        Set(descriptor.SmileFrownLeft_Pos.Value, Tracking.FaceData.SmileFrownLeft);
        Set(descriptor.SmileFrownLeft_Neg.Value, Tracking.FaceData.SmileFrownLeft, true);
        Set(descriptor.SmileFrown_Pos.Value, Tracking.FaceData.SmileFrown);
        Set(descriptor.SmileFrown_Neg.Value, Tracking.FaceData.SmileFrown, true);
        Set(descriptor.SmileSadRight_Pos.Value, Tracking.FaceData.SmileSadRight);
        Set(descriptor.SmileSadRight_Neg.Value, Tracking.FaceData.SmileSadRight, true);
        Set(descriptor.SmileSadLeft_Pos.Value, Tracking.FaceData.SmileSadLeft);
        Set(descriptor.SmileSadLeft_Neg.Value, Tracking.FaceData.SmileSadLeft, true);
        Set(descriptor.SmileSad_Pos.Value, Tracking.FaceData.SmileSad);
        Set(descriptor.SmileSad_Neg.Value, Tracking.FaceData.SmileSad, true);

        Set(descriptor.LipSuckUpper.Value, Tracking.FaceData.LipSuckUpper);
        Set(descriptor.LipSuckLower.Value, Tracking.FaceData.LipSuckLower);
        Set(descriptor.LipSuck.Value, Tracking.FaceData.LipSuck);
        Set(descriptor.LipFunnelUpper.Value, Tracking.FaceData.LipFunnelUpper);
        Set(descriptor.LipFunnelLower.Value, Tracking.FaceData.LipFunnelLower);
        Set(descriptor.LipFunnel.Value, Tracking.FaceData.LipFunnel);
        Set(descriptor.LipPuckerUpper.Value, Tracking.FaceData.LipPuckerUpper);
        Set(descriptor.LipPuckerLower.Value, Tracking.FaceData.LipPuckerLower);
        Set(descriptor.LipPucker.Value, Tracking.FaceData.LipPucker);

        Set(descriptor.NoseSneer.Value, Tracking.FaceData.NoseSneer);
        Set(descriptor.CheekSquint.Value, Tracking.FaceData.CheekSquint);
        Set(descriptor.CheekPuffSuck_Pos.Value, Tracking.FaceData.CheekPuffSuck);
        Set(descriptor.CheekPuffSuck_Neg.Value, Tracking.FaceData.CheekPuffSuck, true);
    }
}