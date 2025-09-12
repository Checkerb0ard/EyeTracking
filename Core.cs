using BoneLib;
using EyeTracking.EyeGaze;

using BoneLib.BoneMenu;
using BoneLib.Notifications;
using Il2CppSLZ.Marrow.Utilities;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(EyeTracking.Core), "EyeTracking", "1.0.2", "Checkerboard")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]
[assembly: MelonOptionalDependencies("LabFusion")]

namespace EyeTracking;

public class Core : MelonMod
{
    internal static Core Instance { get; private set; }
    internal static MelonPreferences_Category Category { get; private set; }
    internal static MelonPreferences_Entry<bool> EnableEyeTracking { get; set; }
    internal static MelonPreferences_Entry<string> ActiveEyeGazeImplementation { get; private set; }
    public static Page? RootPage { get; private set; }
    private static BoolElement? EnableDebug { get; set; }
    public static Page? ImplementationsPage { get; private set; }

    public static bool HasFusion = false;
    
    public override void OnInitializeMelon()
    {
        Instance = this;
        Category = MelonPreferences.CreateCategory("EyeTracking", "Eye Tracking Settings");
        EnableEyeTracking = Category.CreateEntry("EnableEyeTracking", true, "Enable Eye Tracking");
        ActiveEyeGazeImplementation = Category.CreateEntry("ActiveEyeGazeImplementation", "OSC", "Active Eye Gaze Implementation");
        
        Category.SaveToFile(false);
        
        HasFusion = FindMelon("LabFusion", "Lakatrazz") != null;
        
        if (HasFusion)
        {
            Utils.LoadAssemblyFromAssembly(Instance.MelonAssembly.Assembly, "EyeTracking.Resources.EyeTrackingFusion.dll")
                .GetType("EyeTracking.Fusion.ModuleLoader")
                .GetMethod("LoadModule")
                .Invoke(null, null);
        }
        
        RootPage = Page.Root.CreatePage("Eye Tracking", Color.green);
        
#if DEBUG
        EnableDebug = RootPage.CreateBool("Enable Debug", Color.white, false, null);
#endif
        
        RootPage.CreateFunction("Current Implementation: " + ActiveEyeGazeImplementation.Value, Color.green, () =>
        {
            Notifier.Send(new Notification()
            {
                ShowTitleOnPopup = true,
                Title = "hi",
                Message = $"oh, hello there",
                Type = NotificationType.Information,
            });
        });
        
        ImplementationsPage = RootPage.CreatePage("Available Implementations", Color.cyan);
        
        ImplementationManager.Initialize();
    }
    
    public override void OnUpdate()
    {
        if (EnableEyeTracking.Value && ImplementationManager.CurrentImplementation != null)
        {
            ImplementationManager.CurrentImplementation.Update();
            EyeSolver.UpdateBlink();
        }
    }

    public override void OnGUI()
    {
        if (EnableDebug != null && EnableDebug.Value && ImplementationManager.CurrentImplementation != null)
        {
            EyeGazeDebugOverlay.Draw();
        }
    }
}