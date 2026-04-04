using BoneLib.BoneMenu;
using BoneLib.Notifications;
using UnityEngine;

namespace EyeTracking.BoneMenu;

internal class BoneMenuManager
{
    private Page Page { get; set; }
    
    private Page CurrentTrackingProvider { get; set; }
    private FunctionElement ProviderName { get; set; }
    private FunctionElement IsLoaded { get; set; }
    private FunctionElement SupportsEye { get; set; }
    private FunctionElement SupportsFace { get; set; }
    private Page EyeData { get; set; }
    private Page LeftData { get; set; }
    private FunctionElement LeftGaze { get; set; }
    private FunctionElement LeftPupil { get; set; }
    private FunctionElement LeftOpenness { get; set; }
    private Page RightData { get; set; }
    private FunctionElement RightGaze { get; set; }
    private FunctionElement RightPupil { get; set; }
    private FunctionElement RightOpenness { get; set; }
    
    private Page TrackingProviders { get; set; }
    
    internal BoneMenuManager()
    {
        Page = Page.Root.CreatePage("Eye Tracking", Color.green);

        CurrentTrackingProvider = Page.CreatePage($"Current Tracking Provider: {Core.Instance.TrackingProviderManager.CurrentProvider.Name}", Color.green);
        CreateCurrentProviderInfo(CurrentTrackingProvider);
        TrackingProviders = Page.CreatePage("Tracking Providers", Color.cyan);
        CreateProvidersList(TrackingProviders);
    }

    private void CreateCurrentProviderInfo(Page page)
    {
        ProviderName = page.CreateFunction("Name: " + Core.Instance.TrackingProviderManager.CurrentProvider.Name, Color.white, null);
        IsLoaded = page.CreateFunction("Is Loaded: " + Core.Instance.TrackingProviderManager.CurrentProvider.IsLoaded, Color.white, null);
        SupportsEye = page.CreateFunction("Supports Eye Tracking: " + Core.Instance.TrackingProviderManager.CurrentProvider.SupportsEye, Color.white, null);
        SupportsFace = page.CreateFunction("Supports Face Tracking: " + Core.Instance.TrackingProviderManager.CurrentProvider.SupportsFace, Color.white, null);
        
        EyeData = page.CreatePage("Eye Data", Color.green);
        
        LeftData = EyeData.CreatePage("Left Eye", Color.cyan);
        LeftGaze = LeftData.CreateFunction("Gaze: " + new Vector2(Tracking.EyeData.Left.GazeX, Tracking.EyeData.Left.GazeY), Color.white, null);
        LeftPupil = LeftData.CreateFunction("Pupil: " + Tracking.EyeData.Left.PupilDiameterMm, Color.white, null);
        LeftOpenness = LeftData.CreateFunction("Openness: " + Tracking.EyeData.Left.Openness, Color.white, null);
        
        RightData = EyeData.CreatePage("Right Eye", Color.cyan);
        RightGaze = RightData.CreateFunction("Gaze: " + new Vector2(Tracking.EyeData.Right.GazeX, Tracking.EyeData.Right.GazeY), Color.white, null);
        RightPupil = RightData.CreateFunction("Pupil: " + Tracking.EyeData.Right.PupilDiameterMm, Color.white, null);
        RightOpenness = RightData.CreateFunction("Openness: " + Tracking.EyeData.Right.Openness, Color.white, null);
    }

    private void CreateProvidersList(Page page)
    {
        foreach (var provider in Core.Instance.TrackingProviderManager.providers)
            CreateProviderEntry(provider.Name);

        void CreateProviderEntry(string providerName)
        {
            page.CreateFunction(providerName, Color.white, () =>
            {
                Core.Instance.PreferencesManager.SetProvider(providerName);
                
                Notifier.Send(new Notification()
                {
                    ShowTitleOnPopup = true,
                    Title = "Tracking Provider Changed",
                    Message = "Please restart the game for this change to take affect.",
                    Type = NotificationType.Success,
                });
            });
        }
    }

    internal void Update()
    {
        ProviderName?.ElementName = "Name: " + Core.Instance.TrackingProviderManager.CurrentProvider.Name;
        IsLoaded?.ElementName = "Is Loaded: " + Core.Instance.TrackingProviderManager.CurrentProvider.IsLoaded;
        SupportsEye?.ElementName = "Supports Eye Tracking: " + Core.Instance.TrackingProviderManager.CurrentProvider.SupportsEye;
        SupportsFace?.ElementName = "Supports Face Tracking: " + Core.Instance.TrackingProviderManager.CurrentProvider.SupportsFace;
        
        LeftGaze?.ElementName = "Gaze: " + new Vector2(Tracking.EyeData.Left.GazeX, Tracking.EyeData.Left.GazeY);
        LeftPupil?.ElementName = "Pupil: " + Tracking.EyeData.Left.PupilDiameterMm;
        LeftOpenness?.ElementName = "Openness: " + Tracking.EyeData.Left.Openness;
        
        RightGaze?.ElementName = "Gaze: " + new Vector2(Tracking.EyeData.Right.GazeX, Tracking.EyeData.Right.GazeY);
        RightPupil?.ElementName = "Pupil: " + Tracking.EyeData.Right.PupilDiameterMm;
        RightOpenness?.ElementName = "Openness: " + Tracking.EyeData.Right.Openness;
    }
}