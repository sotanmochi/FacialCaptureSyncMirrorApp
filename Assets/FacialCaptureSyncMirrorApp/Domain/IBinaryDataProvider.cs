using System.Threading;
using System.Threading.Tasks;

namespace FacialCaptureSync.MirrorApp
{
    public interface IBinaryDataProvider
    {
        Task<byte[]> LoadAsync(string path, CancellationToken cancellationToken = default);
    }
}