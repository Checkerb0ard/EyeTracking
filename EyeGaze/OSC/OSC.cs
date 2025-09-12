using OscCore;
using UnityEngine;

namespace EyeTracking.EyeGaze.OSC
{
    public class OSC : EyeGazeImplementation
    {
        public override string Name => "OSC";
        public override string DeviceId => "osc";
        public override bool IsLoaded => _isLoaded;

        private bool _isLoaded = false;
        
        private OscReceiver _oscReceiver;
        private OscSender _oscSender;

        private float leftEyeX = 0f;
        private float leftEyeY = 0f;
        private float rightEyeX = 0f;
        private float rightEyeY = 0f;
        private float leftEyeOpen = 1f;
        private float rightEyeOpen = 1f;
        private float leftEyePupilDiameterMm = 2f;
        private float rightEyePupilDiameterMm = 2f;

        public override void Initialize()
        {
            try
            {
                _oscReceiver = new OscReceiver(9000);
                _oscSender = new OscSender(9001);
                
                CreateFakeVRCAvatar();
                _oscSender.Client.Send("/avatar/change", "BONELABEyeTrackingPleaseHelpThisSolutionSucks");

                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/EyeY", (values =>
                {
                    leftEyeY = values.ReadFloatElement(0);
                    rightEyeY = values.ReadFloatElement(0);
                }));
                
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/EyeLeftX", (values =>
                {
                    leftEyeX = values.ReadFloatElement(0);
                }));
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/EyeRightX", (values =>
                {
                    rightEyeX = values.ReadFloatElement(0);
                }));
                
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/EyeOpenLeft", (values =>
                {
                    leftEyeOpen = values.ReadFloatElement(0);
                }));
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/EyeOpenRight", (values =>
                {
                    rightEyeOpen = values.ReadFloatElement(0);
                }));
                
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/PupilDiameterLeft", (values =>
                {
                    leftEyePupilDiameterMm = values.ReadFloatElement(0);
                }));
                _oscReceiver.Server.TryAddMethod("/avatar/parameters/v2/PupilDiameterRight", (values =>
                {
                    rightEyePupilDiameterMm = values.ReadFloatElement(0);
                }));
                
                _isLoaded = true;
            }
            catch (System.Exception ex)
            {
                Core.Instance.LoggerInstance.Error($"Failed to initialize OSC eye tracking: {ex.Message}");
                _isLoaded = false;
            }
        }

        public override void Update()
        {
            if (!_isLoaded)
                return;
            
            Tracking.Data.Eye.Left.Gaze  = new Vector2(leftEyeX,  leftEyeY);
            Tracking.Data.Eye.Right.Gaze = new Vector2(rightEyeX, rightEyeY);
            
            Tracking.Data.Eye.Left.Openness  = leftEyeOpen;
            Tracking.Data.Eye.Right.Openness = rightEyeOpen;

            Tracking.Data.Eye.Left.PupilDiameterMm = leftEyePupilDiameterMm * 10f;
            Tracking.Data.Eye.Right.PupilDiameterMm = rightEyePupilDiameterMm * 10f;
            
            Tracking.Data.Eye.MinDilation = 0f;
            Tracking.Data.Eye.MaxDilation = 10f;
            
            _oscReceiver.Update();
        }

        private void CreateFakeVRCAvatar()
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
            File.WriteAllText(avatarFilePath, "{\n  \"id\": \"BONELABEyeTrackingPleaseHelpThisSolutionSucks\",\n  \"name\": \"BONELAB Eye Tracking Avatar\",\n  \"parameters\": [\n    {\n      \"name\": \"v2/EyeY\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/EyeY\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/EyeY\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/EyeRightX\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/EyeRightX\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/EyeRightX\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/EyeLeftX\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/EyeLeftX\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/EyeLeftX\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/EyeOpenLeft\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/EyeOpenLeft\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/EyeOpenLeft\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/EyeOpenRight\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/EyeOpenRight\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/EyeOpenRight\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/PupilDiameterLeft\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/PupilDiameterLeft\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/PupilDiameterLeft\",\n        \"type\": \"Float\"\n      }\n    },\n    {\n      \"name\": \"v2/PupilDiameterRight\",\n      \"input\": {\n        \"address\": \"/avatar/parameters/v2/PupilDiameterRight\",\n        \"type\": \"Float\"\n      },\n      \"output\": {\n        \"address\": \"/avatar/parameters/v2/PupilDiameterRight\",\n        \"type\": \"Float\"\n      }\n    }\n  ]\n}");
        }
    }
}