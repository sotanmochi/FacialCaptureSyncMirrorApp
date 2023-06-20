using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class AvatarContext
    {
        public event Action OnLoaded;

        public FacialCaptureTarget FacialCaptureTarget { get; private set; }

        private readonly IAvatarResourceProvider _avatarResourceProvider;
        private readonly RuntimeAnimatorController _runtimeAnimatorController;

        public AvatarContext(IAvatarResourceProvider avatarResourceProvider, RuntimeAnimatorController animatorController)
        {
            _avatarResourceProvider = avatarResourceProvider;
            _runtimeAnimatorController = animatorController;
        }

        public async Task LoadAvatarResourceAsync(string path)
        {
            var avatarAnimator = await _avatarResourceProvider.LoadAsync(path);

            avatarAnimator.runtimeAnimatorController = _runtimeAnimatorController;

            FacialCaptureTarget = avatarAnimator.gameObject.AddComponent<FacialCaptureTarget>();
            FacialCaptureTarget.Initialize();

            OnLoaded?.Invoke();
        }
    }
}