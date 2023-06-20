using UnityEngine;
using FacialCaptureSync.MirrorApp.Infrastructure;
using FacialCaptureSync.MirrorApp.Infrastructure.Persistence;

namespace FacialCaptureSync.MirrorApp
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    public class AppMain : MonoBehaviour
    {
        [SerializeField] CaptureSourceConnectionView _captureSourceConnectionView;
        [SerializeField] LoadingVrmView _loadingVrmView;
        [SerializeField] RuntimeAnimatorController _animatorController;

        private IApplicationSettingsRepository _settingsRepository;
        private IAvatarResourceProvider _avatarResourceProvider;
        private IBinaryDataProvider _binaryDataProvider;

        private ApplicationSettings _appsettings;
        private FacialCaptureTarget _captureTarget;

        private FacialCaptureContext _captureContext;
        private AvatarContext _avatarContext;

        private CaptureSourceConnectionPresenter _captureSourceConnectionPresenter;
        private LoadingVrmPresenter _loadingVrmPresenter;

        void Awake()
        {
            UnityEngine.QualitySettings.vSyncCount = 0;
            UnityEngine.Application.targetFrameRate = 60;
            UnityEngine.Application.runInBackground = true;

            _settingsRepository = new ApplicationSettingsLocalRepository();

            _binaryDataProvider = new LocalFileBinaryDataProvider();
            _avatarResourceProvider = new UrpVrmProvider(_binaryDataProvider);

            _avatarContext = new AvatarContext(_avatarResourceProvider, _animatorController);
            _captureContext = new FacialCaptureContext();

            _loadingVrmPresenter = new LoadingVrmPresenter(_loadingVrmView, _avatarContext);
            _captureSourceConnectionPresenter = new CaptureSourceConnectionPresenter(_captureSourceConnectionView, _captureContext);

            _avatarContext.OnLoaded += () =>
            {
                _captureContext.SetCaptureTarget(_avatarContext.FacialCaptureTarget);
            };

            _loadingVrmPresenter.Initialize();
            _captureSourceConnectionPresenter.Initialize();
        }

        async void Start()
        {
            _appsettings = await _settingsRepository.FindAsync();
            if (_appsettings is null)
            {
                _appsettings = new ApplicationSettings()
                {
                    CaptureSourceDeviceAddress = "127.0.0.1",
                    CaptureSourceType = FacialCaptureSourceType.Unknown,
                };
            }
        }

        void LateUpdate()
        {
            _captureContext.Update();
        }

        void OnDestroy()
        {
            _captureContext.Stop();
            _appsettings.CaptureSourceType = _captureContext.CaptureSourceType;
            _appsettings.CaptureSourceDeviceAddress = _captureContext.CaptureDeviceAddress;
            _settingsRepository.SaveAsync(_appsettings);
        }
    }
}