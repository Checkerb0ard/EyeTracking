using System.Reflection;
using OscCore;

namespace EyeTracking.TrackingProviders;

internal class VRCFT : TrackingProvider
{
    public override string Name => "VRCFT";
    public override bool IsLoaded => _isLoaded;
    public override bool SupportsEye => true;
    public override bool SupportsFace => true;
    
    private bool _isLoaded;
    
    private OscReceiver _oscReceiver;
    private OscSender _oscSender;
    
    public override void Initialize()
    {
        try
        {
            _oscReceiver = new OscReceiver();
            _oscSender = new OscSender(9001);
            
            CreateVRCAvatar();
            _oscSender.Client.Send("/avatar/change", "BONELABEyeTrackingPleaseHelpThisSolutionSucks");
            
            BindEyeAddresses();
            BindFaceAddresses();
            
#if false
            _oscReceiver.Server.AddMonitorCallback((address, values) =>
            {
                Core.Instance.LoggerInstance.Msg($"OSC Message: {address} - {string.Join(", ", values.ToString())}");
            });          
#endif
            
            _isLoaded = true;
        }
        catch (Exception ex)
        {
            Core.Instance.LoggerInstance.Error($"Failed to initialize VRCFT tracking provider: {ex}");
            _isLoaded = false;
        }
    }

    public override void Update()
    {
        if (!_isLoaded)
            return;
        
        _oscReceiver.Update();
    }
    
    private void CreateVRCAvatar()
    {
        string appDataLocalLow = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low";
        string vrchatOscPath = Path.Combine(appDataLocalLow, "VRChat", "VRChat", "OSC");
            
        if (!Directory.Exists(vrchatOscPath))
        {
            Directory.CreateDirectory(vrchatOscPath);
        }
            
        string userFolderPath;
        var userDirectories = Directory.GetDirectories(vrchatOscPath);
        
        if (userDirectories.Length > 0)
        {
            userFolderPath = userDirectories[0];
        }
        else
        {
            userFolderPath = Path.Combine(vrchatOscPath, "usr_" + Guid.NewGuid());
            Directory.CreateDirectory(userFolderPath);
        }
            
        string avatarsFolderPath = Path.Combine(userFolderPath, "Avatars");
        if (!Directory.Exists(avatarsFolderPath))
        {
            Directory.CreateDirectory(avatarsFolderPath);
        }

        string avatarFilePath = Path.Combine(avatarsFolderPath, "BONELABEyeTrackingPleaseHelpThisSolutionSucks.json");
            
        const string resourceName = "EyeTracking.Resources.BONELABEyeTrackingPleaseHelpThisSolutionSucks.json";

        var assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream != null)
        {
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            File.WriteAllText(avatarFilePath, json);
        }
    }
    
    private void BindEyeAddresses()
    {
        var server = _oscReceiver.Server;
        var eye = Tracking.EyeData;

        if (server == null)
            return;

        server.TryAddMethod("/avatar/parameters/v2/EyeY", values =>
        {
            var v = values.ReadFloatElement(0);
            eye.Left.GazeY  = v;
            eye.Right.GazeY = v;
        });

        server.TryAddMethod("/avatar/parameters/v2/EyeLeftX", values => eye.Left.GazeX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeRightX", values => eye.Right.GazeX = values.ReadFloatElement(0));

        server.TryAddMethod("/avatar/parameters/v2/EyeOpenLeft", values => eye.Left.Openness = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeOpenRight", values => eye.Right.Openness = values.ReadFloatElement(0));

        server.TryAddMethod("/avatar/parameters/v2/PupilDiameterLeft", values => eye.Left.PupilDiameterMm = values.ReadFloatElement(0) * 10f);
        server.TryAddMethod("/avatar/parameters/v2/PupilDiameterRight", values => eye.Right.PupilDiameterMm = values.ReadFloatElement(0) * 10f);
        
        eye.MinDilation = 0f;
        eye.MaxDilation = 10f;
    }
    
    private void BindFaceAddresses()
    {
        var server = _oscReceiver.Server;
        var face = Tracking.FaceData;
        
        if (server == null)
            return;
        
        server.TryAddMethod("/avatar/parameters/v2/EyeLidRight", values => face.EyeLidRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeLidLeft", values => face.EyeLidLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeLid", values => face.EyeLid = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeSquintRight", values => face.EyeSquintRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeSquintLeft", values => face.EyeSquintLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/EyeSquint", values => face.EyeSquint = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowPinchRight", values => face.BrowPinchRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowPinchLeft", values => face.BrowPinchLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowLowererRight", values => face.BrowLowererRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowLowererLeft", values => face.BrowLowererLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowInnerUpRight", values => face.BrowInnerUpRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowInnerUpLeft", values => face.BrowInnerUpLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowOuterUpRight", values => face.BrowOuterUpRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowOuterUpLeft", values => face.BrowOuterUpLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NoseSneerRight", values => face.NoseSneerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NoseSneerLeft", values => face.NoseSneerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NasalDilationRight", values => face.NasalDilationRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NasalDilationLeft", values => face.NasalDilationLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NasalConstrictRight", values => face.NasalConstrictRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NasalConstrictLeft", values => face.NasalConstrictLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekSquintRight", values => face.CheekSquintRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekSquintLeft", values => face.CheekSquintLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekPuffSuckRight", values => face.CheekPuffSuckRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekPuffSuckLeft", values => face.CheekPuffSuckLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/JawOpen", values => face.JawOpen = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthClosed", values => face.MouthClosed = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/JawX", values => face.JawX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/JawZ", values => face.JawZ = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/JawClench", values => face.JawClench = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/JawMandibleRaise", values => face.JawMandibleRaise = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckUpperRight", values => face.LipSuckUpperRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckUpperLeft", values => face.LipSuckUpperLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckLowerRight", values => face.LipSuckLowerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckLowerLeft", values => face.LipSuckLowerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckCornerRight", values => face.LipSuckCornerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckCornerLeft", values => face.LipSuckCornerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelUpperRight", values => face.LipFunnelUpperRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelUpperLeft", values => face.LipFunnelUpperLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelLowerRight", values => face.LipFunnelLowerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelLowerLeft", values => face.LipFunnelLowerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerUpperRight", values => face.LipPuckerUpperRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerUpperLeft", values => face.LipPuckerUpperLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerLowerRight", values => face.LipPuckerLowerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerLowerLeft", values => face.LipPuckerLowerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperUpRight", values => face.MouthUpperUpRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperUpLeft", values => face.MouthUpperUpLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthLowerDownRight", values => face.MouthLowerDownRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthLowerDownLeft", values => face.MouthLowerDownLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperDeepenRight", values => face.MouthUpperDeepenRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperDeepenLeft", values => face.MouthUpperDeepenLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperX", values => face.MouthUpperX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthLowerX", values => face.MouthLowerX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthCornerPullRight", values => face.MouthCornerPullRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthCornerPullLeft", values => face.MouthCornerPullLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthCornerSlantRight", values => face.MouthCornerSlantRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthCornerSlantLeft", values => face.MouthCornerSlantLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthDimpleRight", values => face.MouthDimpleRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthDimpleLeft", values => face.MouthDimpleLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthFrownRight", values => face.MouthFrownRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthFrownLeft", values => face.MouthFrownLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthStretchRight", values => face.MouthStretchRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthStretchLeft", values => face.MouthStretchLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthRaiserUpper", values => face.MouthRaiserUpper = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthRaiserLower", values => face.MouthRaiserLower = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthPressRight", values => face.MouthPressRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthPressLeft", values => face.MouthPressLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthTightenerRight", values => face.MouthTightenerRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthTightenerLeft", values => face.MouthTightenerLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueOut", values => face.TongueOut = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueX", values => face.TongueX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueY", values => face.TongueY = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueRoll", values => face.TongueRoll = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueArchY", values => face.TongueArchY = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueShape", values => face.TongueShape = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueTwistRight", values => face.TongueTwistRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/TongueTwistLeft", values => face.TongueTwistLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SoftPalateClose", values => face.SoftPalateClose = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/ThroatSwallow", values => face.ThroatSwallow = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NeckFlexRight", values => face.NeckFlexRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NeckFlexLeft", values => face.NeckFlexLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowDownRight", values => face.BrowDownRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowDownLeft", values => face.BrowDownLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowOuterUp", values => face.BrowOuterUp = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowInnerUp", values => face.BrowInnerUp = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowUp", values => face.BrowUp = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowExpressionRight", values => face.BrowExpressionRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowExpressionLeft", values => face.BrowExpressionLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/BrowExpression", values => face.BrowExpression = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthX", values => face.MouthX = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthUpperUp", values => face.MouthUpperUp = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthLowerDown", values => face.MouthLowerDown = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthOpen", values => face.MouthOpen = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthSmileRight", values => face.MouthSmileRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthSmileLeft", values => face.MouthSmileLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthSadRight", values => face.MouthSadRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/MouthSadLeft", values => face.MouthSadLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileFrownRight", values => face.SmileFrownRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileFrownLeft", values => face.SmileFrownLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileFrown", values => face.SmileFrown = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileSadRight", values => face.SmileSadRight = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileSadLeft", values => face.SmileSadLeft = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/SmileSad", values => face.SmileSad = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckUpper", values => face.LipSuckUpper = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuckLower", values => face.LipSuckLower = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipSuck", values => face.LipSuck = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelUpper", values => face.LipFunnelUpper = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnelLower", values => face.LipFunnelLower = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipFunnel", values => face.LipFunnel = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerUpper", values => face.LipPuckerUpper = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPuckerLower", values => face.LipPuckerLower = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/LipPucker", values => face.LipPucker = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/NoseSneer", values => face.NoseSneer = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekSquint", values => face.CheekSquint = values.ReadFloatElement(0));
        server.TryAddMethod("/avatar/parameters/v2/CheekPuffSuck", values => face.CheekPuffSuck = values.ReadFloatElement(0));
    }
}