namespace FacialCaptureSync.MirrorApp
{
    public class FacialCaptureContext
    {
        public FacialCaptureSourceType CaptureSourceType { get; private set; }
        public string CaptureDeviceAddress { get; private set; }

        private IFacialCaptureSource _captureSource;
        private FacialCaptureReceiver _captureReceiver;
        private FacialCaptureTarget _target;
        private FacialCapture _capture = new();

        public void Start(FacialCaptureSourceType captureSourceType, string captureDeviceAddress)
        {
            CaptureSourceType = captureSourceType;
            CaptureDeviceAddress = captureDeviceAddress;

            _captureSource = captureSourceType switch
            {
                FacialCaptureSourceType.Facemotion3d => new Facemotion3d(),
                FacialCaptureSourceType.iFacialMocap => new iFacialMocap(),
                FacialCaptureSourceType.iFacialMocap_PCApp => new iFacialMocap(useIndirectConnection: true),
                _ => null,
            };

            _captureReceiver = new FacialCaptureReceiver(_captureSource);
            _captureReceiver.Start();
            _captureReceiver.ConnectToCaptureSource(captureDeviceAddress);
        }

        public void Reconnect()
        {
            _captureReceiver.ConnectToCaptureSource(CaptureDeviceAddress);
        }

        public void Stop()
        {
            _captureReceiver.Dispose();
            _captureReceiver = null;
        }

        public void Update()
        {
            _captureReceiver?.TryDequeueCapture(ref _capture);
            _target?.SetBlendShapes(_capture);
            _target?.SetBonePoses(_capture);
        }

        public void SetCaptureTarget(FacialCaptureTarget target)
        {
            _target = target;
        }

        public void UnsetCaptureTarget()
        {
            _target = null;
        }
    }
}