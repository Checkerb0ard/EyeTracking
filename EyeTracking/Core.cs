using EyeTracking.BoneMenu;
using EyeTracking.Compatibility;
using EyeTracking.Solvers;
using EyeTracking.TrackingProviders;
using EyeTracking.UserData;
using MelonLoader;

[assembly: MelonInfo(typeof(EyeTracking.Core), EyeTracking.BuildInfo.Name, EyeTracking.BuildInfo.Version, EyeTracking.BuildInfo.Author)]
[assembly: MelonGame(EyeTracking.BuildInfo.GameDeveloper, EyeTracking.BuildInfo.GameName)]

namespace EyeTracking;

public class Core : MelonMod
{
    internal static Core Instance { get; private set; }
    
    internal PreferencesManager PreferencesManager { get; private set; }
    internal TrackingProviderManager TrackingProviderManager { get; private set; }
    internal BoneMenuManager BoneMenuManager { get; private set; }
    internal SolverManager SolverManager { get; private set; }
    
    public override void OnInitializeMelon()
    {
        Instance = this;
        
        PreferencesManager = new PreferencesManager();
        TrackingProviderManager = new TrackingProviderManager();
        BoneMenuManager = new BoneMenuManager();
        SolverManager = new SolverManager();
        
        FusionModuleManager.TryLoadModule(out _);
    }
    
    public override void OnUpdate()
    {
        if (!PreferencesManager.Enabled.Value)
            return;
        
        BoneMenuManager.Update();
        SolverManager.Update();
    }
}