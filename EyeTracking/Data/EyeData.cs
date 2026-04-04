namespace EyeTracking.Data;

public class EyeData
{
    public readonly SingleEyeData Left = new SingleEyeData();
    public readonly SingleEyeData Right = new SingleEyeData();
    public float MaxDilation;
    public float MinDilation = 10f;
    
    private readonly SingleEyeData _combined = new();

    public SingleEyeData Combined() 
    {
        float avgPupil = (Left.PupilDiameterMm + Right.PupilDiameterMm) / 2f;
        if (avgPupil < MinDilation) MinDilation = avgPupil;
        if (avgPupil > MaxDilation) MaxDilation = avgPupil;

        _combined.GazeX = (Left.GazeX + Right.GazeX) / 2f;
        _combined.GazeY = (Left.GazeY + Right.GazeY) / 2f;
        _combined.Openness = (Left.Openness + Right.Openness) / 2f;
        _combined.PupilDiameterMm = (avgPupil - MinDilation) / (MaxDilation - MinDilation);
        return _combined;
    }
}