using System;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class CaptureSourceConnectionPresenter
    {
        private readonly CaptureSourceConnectionView _view;
        private readonly FacialCaptureContext _captureContext;

        public CaptureSourceConnectionPresenter
        (
            CaptureSourceConnectionView view,
            FacialCaptureContext captureContext
        )
        {
            _view = view;
            _captureContext = captureContext;
        }

        public void Initialize()
        {
            var captureSourceTypes = Enum.GetNames(typeof(FacialCaptureSourceType));
            var currentSourceTypeIndex = Array.IndexOf(captureSourceTypes, _captureContext.CaptureSourceType.ToString());

            _view.UpdateDropdownOptions(captureSourceTypes);
            _view.SetCaptureSourceType(currentSourceTypeIndex);
            _view.SetIpAddress(_captureContext.CaptureDeviceAddress);

            _view.OnClickConnect += properties => 
            {
                var captureSourceType = FacialCaptureSourceType.iFacialMocap;

                if (Enum.TryParse(typeof(FacialCaptureSourceType), properties.CaptureSourceTypeName, out var result))
                {
                    captureSourceType = (FacialCaptureSourceType)result;
                }

                _captureContext.Start(captureSourceType, properties.CaptureSourceDeviceIpAddress);
            };
        }
    }
}