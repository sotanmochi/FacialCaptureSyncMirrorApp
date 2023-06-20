using System.Threading;
using System.Threading.Tasks;

namespace FacialCaptureSync.MirrorApp
{
    public interface IAvatarResourceProvider
    {
        Task<UnityEngine.Animator> LoadAsync(string path, CancellationToken cancellationToken = default);
    }
}