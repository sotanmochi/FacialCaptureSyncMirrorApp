using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace FacialCaptureSync.MirrorApp.Infrastructure.Persistence
{
    public sealed class ApplicationSettingsLocalRepository : IApplicationSettingsRepository
    {
        private readonly string _directoryPath;
        private readonly string _filename;

        public ApplicationSettingsLocalRepository(string directoryPath = "", string filename = "appsettings.json")
        {
            _directoryPath = string.IsNullOrEmpty(directoryPath) ? Application.persistentDataPath : directoryPath;
            _filename = filename;
        }

        public async Task<ApplicationSettings> FindAsync()
        {
            var path = Path.Combine(_directoryPath, _filename);
            if (File.Exists(path))
            {
                var text = await File.ReadAllTextAsync(path, System.Text.Encoding.ASCII);
                return JsonUtility.FromJson<ApplicationSettings>(text);
            }
            return null;
        }

        public async Task<bool> SaveAsync(ApplicationSettings value)
        {
            var json = JsonUtility.ToJson(value);
            var path = Path.Combine(_directoryPath, _filename);
            await File.WriteAllTextAsync(path, json, System.Text.Encoding.ASCII);
            return true;
        }
    }
}