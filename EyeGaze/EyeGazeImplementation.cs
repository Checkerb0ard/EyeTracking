namespace EyeTracking.EyeGaze;

public abstract class EyeGazeImplementation
{
    public abstract string Name { get; }
    public abstract string DeviceId { get; }
    
    public abstract void Initialize();
    public abstract void Update();
}