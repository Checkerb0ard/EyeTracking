using EyeTracking.Data;

namespace EyeTracking;

public static class Tracking
{
    public static bool IsTracking => Core.Instance.TrackingProviderManager.CurrentProvider != null && Core.Instance.TrackingProviderManager.CurrentProvider.IsLoaded;
    public static bool SupportsEye => Core.Instance.TrackingProviderManager.CurrentProvider.SupportsEye;
    public static bool SupportsFace => Core.Instance.TrackingProviderManager.CurrentProvider.SupportsFace;
    public static readonly EyeData EyeData = new();
    public static readonly FaceData FaceData = new();
}