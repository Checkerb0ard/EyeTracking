namespace EyeTracking.TrackingProviders;

/// <summary>
/// An eye and face data source.
/// </summary>
public abstract class TrackingProvider
{
    /// <summary>
    /// Friendly name of the provider.
    /// </summary>
    public abstract string Name { get; }
    /// <summary>
    /// Have we fulled loaded and started tracking?
    /// </summary>
    public abstract bool IsLoaded { get; }
    /// <summary>
    /// Does this provider support eye tracking?
    /// </summary>
    public abstract bool SupportsEye { get; }
    /// <summary>
    /// Does this provider support face tracking?
    /// </summary>
    public abstract bool SupportsFace { get; }
    
    /// <summary>
    /// Called when this provider is loaded as the active provider.
    /// </summary>
    public abstract void Initialize();
    /// <summary>
    /// This method is called on a separate thread and should contain the main loop for fetching and updating tracking data.
    /// </summary>
    public abstract void Update();
}