using System;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class CaptureSourceConnectionPresenter
    {
        readonly CaptureSourceConnectionView _view;
        readonly FacialCaptureContext _context;

        public CaptureSourceConnectionPresenter
        (
            CaptureSourceConnectionView view,
            FacialCaptureContext context
        )
        {
            _view = view;
            _context = context;
        }

        public void Initialize()
        {
            _view.UpdateDropdownOptions(Enum.GetNames(typeof(FacialCaptureSourceType)));

            _view.OnClickConnect += properties => 
            {
                var captureSourceType = FacialCaptureSourceType.Unknown;

                if (Enum.TryParse(typeof(FacialCaptureSourceType), properties.CaptureSourceTypeName, out var result))
                {
                    captureSourceType = (FacialCaptureSourceType)result;
                }

                _context.Start(captureSourceType, properties.CaptureSourceDeviceIpAddress);
            };
        }
    }
}