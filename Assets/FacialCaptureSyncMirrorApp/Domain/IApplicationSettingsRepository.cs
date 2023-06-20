using System.Threading.Tasks;

namespace FacialCaptureSync.MirrorApp
{
    public interface IApplicationSettingsRepository
    {
        Task<ApplicationSettings> FindAsync();
        Task SaveAsync(ApplicationSettings value);
    }
}