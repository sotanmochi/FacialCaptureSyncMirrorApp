using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace FacialCaptureSync.MirrorApp.Infrastructure.Persistence
{
    public sealed class ApplicationSettingsLocalRepository : IApplicationSettingsRepository
    {
        public async Task<ApplicationSettings> FindAsync()
        {
            var path = Path.Combine(Application.persistentDataPath, "appsettings.json");
            if (File.Exists(path))
            {
                var text = await File.ReadAllTextAsync(path, System.Text.Encoding.ASCII);
                return JsonUtility.FromJson<ApplicationSettings>(text);
            }
            return null;
        }

        public async Task SaveAsync(ApplicationSettings value)
        {
            var json = JsonUtility.ToJson(value);
            var path = Path.Combine(Application.persistentDataPath, "appsettings.json");
            await File.WriteAllTextAsync(path, json, System.Text.Encoding.ASCII);
        }
    }
}