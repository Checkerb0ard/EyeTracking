using MelonLoader;

namespace EyeTracking.UserData;

internal class PreferencesManager
{
    private MelonPreferences_Category Category { get; set; }
    
    internal MelonPreferences_Entry<bool> Enabled { get; private set; }
    internal MelonPreferences_Entry<string> TrackingProvider { get; private set; }
    
    internal PreferencesManager()
    {
        Category = MelonPreferences.CreateCategory("EyeTracking");
        
        Enabled = Category.CreateEntry("Enabled", true);
        TrackingProvider = Category.CreateEntry("TrackingProvider", "VRCFT");
        
        Save();
    }

    internal void SetProvider(string provider)
    {
        TrackingProvider.Value = provider;
        Save();
    }
    
    internal void Save() => Category.SaveToFile(false);
}