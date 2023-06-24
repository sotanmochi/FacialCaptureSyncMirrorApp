using System;

namespace FacialCaptureSync.MirrorApp
{
    [Serializable]
    public class ApplicationSettings
    {
        public string CaptureSourceDeviceAddress = "127.0.0.1";
        public FacialCaptureSourceType CaptureSourceType = FacialCaptureSourceType.iFacialMocap;
    }
}