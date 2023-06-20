using System;

namespace FacialCaptureSync.MirrorApp
{
    [Serializable]
    public class ApplicationSettings
    {
        public string CaptureSourceDeviceAddress;
        public FacialCaptureSourceType CaptureSourceType;
    }
}