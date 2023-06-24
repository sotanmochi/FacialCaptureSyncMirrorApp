using System.Threading.Tasks;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class ApplicationContext
    {
        public ApplicationSettings ApplicationSettings => _applicationSettings;

        private readonly IApplicationSettingsRepository _settingsRepository;
        private readonly ApplicationSettings _applicationSettings = new();

        public ApplicationContext(IApplicationSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task InitializeAsync()
        {
            await LoadSettingsAsync();
        }

        private async Task<bool> LoadSettingsAsync()
        {
            var settings = await _settingsRepository.FindAsync();
            if (settings is not null)
            {
                _applicationSettings.CaptureSourceDeviceAddress = settings.CaptureSourceDeviceAddress;
                _applicationSettings.CaptureSourceType = settings.CaptureSourceType;
                return true;
            }            
            return false;
        }

        public async Task<bool> SaveSettingsAsync(string captureSourceDeviceAddress, FacialCaptureSourceType captureSourceType)
        {
            _applicationSettings.CaptureSourceDeviceAddress = captureSourceDeviceAddress;
            _applicationSettings.CaptureSourceType = captureSourceType;
            return await SaveSettingsAsync();
        }

        public async Task<bool> SaveSettingsAsync()
        {
            DebugLog($"SaveSettingsAsync @Thread:{System.Environment.CurrentManagedThreadId}");
            return await _settingsRepository.SaveAsync(_applicationSettings);
        }

        [
            System.Diagnostics.Conditional("UNITY_EDITOR"),
            System.Diagnostics.Conditional("DEVELOPMENT_BUILD"),
        ]
        private static void DebugLog(object message)
        {
            UnityEngine.Debug.Log($"[DEBUG] [{nameof(ApplicationContext)}] {message}");
        }
    }
}