using EyeTracking.Utilities;

namespace EyeTracking.TrackingProviders;

internal class TrackingProviderManager
{
    internal TrackingProvider CurrentProvider { get; private set; }
    
    internal readonly List<TrackingProvider> providers = new();
    private static readonly string[] blackListedAssemblyNames =
    [
        "Il2Cpp",
        "Unity",
    ];
    
    private Thread _trackingThread;
    private volatile bool _trackingThreadRunning;
    
    internal TrackingProviderManager()
    {
        LoadProviders();
        
        if (!LoadProvider(Core.Instance.PreferencesManager.TrackingProvider.Value))
            LoadFallbackProvider();
        
        StartThread();
    }

    private void LoadProviders()
    {
        providers.Clear();
        CurrentProvider = null;
        
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (blackListedAssemblyNames.Any(blackListedName => assembly.FullName != null && assembly.FullName.Contains(blackListedName)))
                continue;
            
            AssemblyUtilities.LoadAllValid<TrackingProvider>(assembly, RegisterProvider);
        }

        void RegisterProvider(Type type)
        {
            TrackingProvider provider = Activator.CreateInstance(type) as TrackingProvider;
            
            if (provider == null)
                return;
            
            providers.Add(provider);
        }
    }

    private bool LoadProvider(string name)
    {
        var provider = providers.FirstOrDefault(p => p.Name == name);
        
        if (provider == null)
            return false;
        
        CurrentProvider = provider;
        CurrentProvider.Initialize();
        
        return true;
    }

    private void LoadFallbackProvider()
    {
        Core.Instance.PreferencesManager.SetProvider(Core.Instance.PreferencesManager.TrackingProvider.DefaultValue);
        
        LoadProvider(Core.Instance.PreferencesManager.TrackingProvider.Value);
    }

    internal void StartThread()
    {
        _trackingThreadRunning = true;
        _trackingThread = new Thread(() =>
        {
            while (_trackingThreadRunning)
            {
                CurrentProvider.Update();
                Thread.Sleep(10);
            }
        })
        {
            Name = "EyeTracking_ProviderThread",
            IsBackground = true
        };
        _trackingThread.Start();
    }
}