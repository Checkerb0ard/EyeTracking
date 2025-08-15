namespace EyeTracking.Data;

public class EyeData
{
    public readonly SingleEyeData Left = new SingleEyeData();
    public readonly SingleEyeData Right = new SingleEyeData();
    public float MaxDilation;
    public float MinDilation = 999f;
    
    public SingleEyeData Combined()
    {
        if ((Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f < MinDilation)
            MinDilation = (Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f;
        if ((Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f > MaxDilation)
            MaxDilation = (Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f;

        return new SingleEyeData
        {
            Gaze = (Left.Gaze + Right.Gaze) / 2f,
            Openness = (Left.Openness + Right.Openness) / 2f,
            PupilDiameterMm = ((Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f - MinDilation) / (MaxDilation - MinDilation)
        };
    }
    
    public void CopyPropertiesOf(EyeData data)
    {
        Left.Gaze = data.Left.Gaze;
        Left.Openness = data.Left.Openness;
        Left.PupilDiameterMm = data.Left.PupilDiameterMm;
        Right.Gaze = data.Right.Gaze;
        Right.Openness = data.Right.Openness;
        Right.PupilDiameterMm = data.Right.PupilDiameterMm;
        MaxDilation = data.MaxDilation;
        MinDilation = data.MinDilation;
    }
}