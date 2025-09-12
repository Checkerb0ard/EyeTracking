using EyeTracking.EyeGaze;

using BoneLib.Notifications;

using UnityEngine;

namespace EyeTracking;

public static class ImplementationManager
{
    private static readonly List<EyeGazeImplementation> Implementations = new  List<EyeGazeImplementation>();
    public static EyeGazeImplementation? CurrentImplementation { get; private set; }
    
    internal static void Initialize()
    {
        LoadImplementations();

        // not good. the mod contains a default implementation, so we should always have at least one implementation.
        if (CurrentImplementation == null)
        {
            Core.ActiveEyeGazeImplementation.Value = Core.ActiveEyeGazeImplementation.DefaultValue;
            Core.Category.SaveToFile(false);
            
            LoadImplementations();
        }

        foreach (var implementation in Implementations)
        {
            Core.ImplementationsPage.CreateFunction(implementation.Name, Color.white, (() =>
            {
                Core.ActiveEyeGazeImplementation.Value = implementation.Name;
                Core.Category.SaveToFile(false);
                
                Notifier.Send(new Notification()
                {
                    ShowTitleOnPopup = true,
                    Title = "Eye Tracking Implementation Changed",
                    Message = $"Please restart the game for this change to take affect.",
                    Type = NotificationType.Success,
                });
            }));
        }
    }
    
    private static void LoadImplementations()
    {
        Implementations.Clear();
        CurrentImplementation = null;
        
#if DEBUG
        Core.Instance.LoggerInstance.Msg($"Populating Implementation List!");
#endif

        foreach (var melon in MelonLoader.MelonBase.RegisteredMelons)
        {
            Utils.LoadAllValid<EyeGazeImplementation>(melon.MelonAssembly.Assembly, RegisterImplementation);
        }
    }

    private static void RegisterImplementation(Type type)
    {
        EyeGazeImplementation implementation = Activator.CreateInstance(type) as EyeGazeImplementation;

        if (implementation == null)
        {
#if DEBUG
            Core.Instance.LoggerInstance.Msg($"Failed to register Implementation {type.Name} as it is not a valid Implementation!");
#endif
        }
        else
        {
#if DEBUG
            Core.Instance.LoggerInstance.Msg($"Registered Implementation {type.Name}");
#endif

            if (implementation.Name == Core.ActiveEyeGazeImplementation.Value)
            {
                Core.Instance.LoggerInstance.Msg($"Loaded Implementation {implementation.Name} as the active Implementation.");
                CurrentImplementation = implementation;
                CurrentImplementation.Initialize();
            }
            
            Implementations.Add(implementation);
        }
    }
}