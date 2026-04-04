using OscCore;

namespace EyeTracking.TrackingProviders;

internal class Babble : TrackingProvider
{
    public override string Name => "Babble";
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
            _oscReceiver = new OscReceiver(8888);
            _oscSender = new OscSender(8889);
            
            BindEyeAddresses();
            
#if true
            _oscReceiver.Server.AddMonitorCallback((address, values) =>
            {
                Core.Instance.LoggerInstance.Msg($"OSC Message: {address} - {string.Join(", ", values.ToString())}");
            });          
#endif
            
            _isLoaded = true;
        }
        catch (Exception ex)
        {
            Core.Instance.LoggerInstance.Error($"Failed to initialize Babble tracking provider: {ex}");
            _isLoaded = false;
        }
    }

    public override void Update()
    {
        if (!_isLoaded)
            return;
        
        _oscReceiver.Update();
    }
    
    private void BindEyeAddresses()
    {
        var server = _oscReceiver.Server;
        var eye = Tracking.EyeData;

        if (server == null)
            return;
        
        server.TryAddMethod("/LeftEyeX", values => eye.Left.GazeX = values.ReadFloatElement(0));
        server.TryAddMethod("/LeftEyeY", values => eye.Left.GazeY = values.ReadFloatElement(0));
        server.TryAddMethod("/LeftEyeLid", values => eye.Left.Openness = values.ReadFloatElement(0));
        
        server.TryAddMethod("/RightEyeX", values => eye.Right.GazeX = values.ReadFloatElement(0));
        server.TryAddMethod("/RightEyeY", values => eye.Right.GazeY = values.ReadFloatElement(0));
        server.TryAddMethod("/RightEyeLid", values => eye.Right.Openness = values.ReadFloatElement(0));

        // No pupil dilation :(
        eye.Left.PupilDiameterMm = 2f;
        eye.Right.PupilDiameterMm = 2f;
        
        eye.MinDilation = 0f;
        eye.MaxDilation = 10f;
    }
}