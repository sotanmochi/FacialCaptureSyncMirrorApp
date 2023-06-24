namespace FacialCaptureSync.MirrorApp
{
    public sealed class FacialCaptureContext
    {
        public bool IsRunning { get; private set; }

        public FacialCaptureSourceType CaptureSourceType { get; private set; }
        public string CaptureDeviceAddress { get; private set; }

        private readonly ApplicationContext _applicationContext;

        private IFacialCaptureSource _captureSource;
        private FacialCaptureReceiver _captureReceiver;
        private FacialCaptureTarget _target;
        private FacialCapture _capture = new();

        public FacialCaptureContext(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void Initialize()
        {
            CaptureSourceType = _applicationContext.ApplicationSettings.CaptureSourceType;
            CaptureDeviceAddress = _applicationContext.ApplicationSettings.CaptureSourceDeviceAddress;
        }

        public void Start(FacialCaptureSourceType captureSourceType, string captureDeviceAddress)
        {
            IsRunning = true;
            CaptureSourceType = captureSourceType;
            CaptureDeviceAddress = captureDeviceAddress;

            _captureSource = captureSourceType switch
            {
                FacialCaptureSourceType.Facemotion3d => new Facemotion3d(),
                FacialCaptureSourceType.iFacialMocap => new iFacialMocap(),
                FacialCaptureSourceType.iFacialMocap_PCApp => new iFacialMocap(useIndirectConnection: true),
                _ => new iFacialMocap()
            };

            _captureReceiver = new FacialCaptureReceiver(_captureSource);
            _captureReceiver.Start();
            _captureReceiver.ConnectToCaptureSource(captureDeviceAddress);
        }

        public void Connect()
        {
            _captureReceiver?.ConnectToCaptureSource(CaptureDeviceAddress);
        }

        public void Stop()
        {
            IsRunning = false;
            _captureReceiver?.Dispose();
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