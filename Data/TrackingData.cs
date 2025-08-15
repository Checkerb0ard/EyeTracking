namespace EyeTracking.Data;

public class TrackingData
{
    public readonly EyeData Eye = new  EyeData();
    
    public void CopyPropertiesOf(TrackingData data)
    {
        Eye.CopyPropertiesOf(data.Eye);
    }
}