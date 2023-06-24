using System;
using UnityEngine;
using PlayerLooper;
using FacialCaptureSync.MirrorApp.Infrastructure;
using FacialCaptureSync.MirrorApp.Infrastructure.Persistence;

namespace FacialCaptureSync.MirrorApp
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    public sealed class AppMain : MonoBehaviour, IInitializable, IStartable, IPostLateUpdatable, IDisposable
    {
        [SerializeField] CaptureSourceConnectionView _captureSourceConnectionView;
        [SerializeField] LoadingVrmView _loadingVrmView;
        [SerializeField] RuntimeAnimatorController _animatorController;

        private IAvatarResourceProvider _avatarResourceProvider;
        private IBinaryDataProvider _binaryDataProvider;

        private ApplicationContext _applicationContext;
        private FacialCaptureContext _captureContext;
        private AvatarContext _avatarContext;

        private CaptureSourceConnectionPresenter _captureSourceConnectionPresenter;
        private LoadingVrmPresenter _loadingVrmPresenter;

        void Awake()
        {
            GlobalPlayerLooper.Register(this);
        }

        void IInitializable.Initialize()
        {
            DebugLog("Initializing");

            UnityEngine.QualitySettings.vSyncCount = 0;
            UnityEngine.Application.targetFrameRate = 60;
            UnityEngine.Application.runInBackground = true;

            _binaryDataProvider = new LocalFileBinaryDataProvider();
            _avatarResourceProvider = new UrpVrmProvider(_binaryDataProvider);

            _applicationContext = new ApplicationContext(new ApplicationSettingsLocalRepository());
            _captureContext = new FacialCaptureContext(_applicationContext);
            _avatarContext = new AvatarContext(_avatarResourceProvider, _animatorController);

            _captureSourceConnectionPresenter = new CaptureSourceConnectionPresenter(_captureSourceConnectionView, _captureContext);
            _loadingVrmPresenter = new LoadingVrmPresenter(_loadingVrmView, _avatarContext);

            DebugLog("Initialized");
        }

        async void IStartable.Start()
        {
            DebugLog("Starting");

            _avatarContext.OnLoaded += () =>
            {
                _captureContext.SetCaptureTarget(_avatarContext.FacialCaptureTarget);
            };

            await _applicationContext.InitializeAsync();
            _captureContext.Initialize();

            _captureSourceConnectionPresenter.Initialize();
            _loadingVrmPresenter.Initialize();

            DebugLog("Started");
        }

        void IPostLateUpdatable.PostLateUpdate()
        {
            _captureContext.Update();
        }

        async void IDisposable.Dispose()
        {
            DebugLog("Disposing");

            _captureContext.Stop();
            await _applicationContext.SaveSettingsAsync(_captureContext.CaptureDeviceAddress, _captureContext.CaptureSourceType);

            DebugLog("Disposed");
        }

        [
            System.Diagnostics.Conditional("UNITY_EDITOR"),
            System.Diagnostics.Conditional("DEVELOPMENT_BUILD"),
        ]
        private static void DebugLog(object message)
        {
            UnityEngine.Debug.Log($"[DEBUG] [{nameof(AppMain)}] {message}");
        }
    }
}