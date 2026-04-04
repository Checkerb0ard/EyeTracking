using System.Reflection;
using EyeTracking.Utilities;
using MelonLoader;

namespace EyeTracking.Compatibility;

internal static class FusionModuleManager
{
    private const string MelonName = "LabFusion";
    private const string MelonAuthor = "Lakatrazz";

    private const string ResourceName = "EyeTracking.Resources.EyeTrackingFusionModule.dll";
    
    internal static void TryLoadModule(out bool loaded)
    {
        bool hasFusion = MelonBase.FindMelon(MelonName, MelonAuthor) != null;
        loaded = hasFusion;
        
        if (!hasFusion)
            return;
        
        var rawAssembly = EmbeddedResourceUtilities.LoadBytesFromAssembly(Core.Instance.MelonAssembly.Assembly, ResourceName);
        if (rawAssembly == null || rawAssembly.Length == 0)
            return;
        
        var assembly = Assembly.Load(rawAssembly);
        
        assembly.GetType("EyeTracking.Fusion.Core")
            ?.GetMethod("LoadModule")
            ?.Invoke(null, null);
    }
}