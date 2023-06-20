using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UniGLTF;
using UniVRM10;

namespace FacialCaptureSync.MirrorApp.Infrastructure
{
    public sealed class UrpVrmProvider : IAvatarResourceProvider
    {
        readonly IBinaryDataProvider _binaryDataProvider;

        public UrpVrmProvider(IBinaryDataProvider binaryDataProvider)
        {
            _binaryDataProvider = binaryDataProvider;
        }

        public async Task<Animator> LoadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            try
            {
                var bytes = await _binaryDataProvider.LoadAsync(path, cancellationToken);

                var loadedVrm = await Vrm10.LoadBytesAsync(bytes,
                    canLoadVrm0X: true,
                    showMeshes: false,
                    materialGenerator: new UrpVrm10MaterialDescriptorGenerator(),
                    ct: cancellationToken);

                if (loadedVrm == null)
                {
                    return null;
                }

                var instance = loadedVrm.GetComponent<RuntimeGltfInstance>();
                instance.ShowMeshes();
                instance.EnableUpdateWhenOffscreen();

                return loadedVrm.GetComponent<Animator>();
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}