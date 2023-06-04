using UnityEngine;

namespace FacialCaptureSyncMirrorApp
{
    /// <summary>
    /// Entry point of the application.
    /// </summary>
    public class AppMain : MonoBehaviour
    {
        void Awake()
        {
            UnityEngine.QualitySettings.vSyncCount = 0;
            UnityEngine.Application.targetFrameRate = 60;
            UnityEngine.Application.runInBackground = true;

            Debug.Log($"[{nameof(AppMain)}] Awake()");
        }
    }
}