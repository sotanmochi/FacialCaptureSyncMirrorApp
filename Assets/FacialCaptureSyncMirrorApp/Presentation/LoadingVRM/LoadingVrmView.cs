using System;
using UnityEngine;
using UnityEngine.UI;
using UniVRM10.URPSample;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class LoadingVrmView : MonoBehaviour
    {
        [SerializeField] Button _open;

        public event Action<string> OnOpenLocalFile;

        void Awake()
        {
            _open.onClick.AddListener(OnClickOpenEventHandler);
        }

        void OnDestroy()
        {
            _open.onClick.RemoveAllListeners();
        }

        private void OnClickOpenEventHandler()
        {
#if UNITY_STANDALONE_WIN
            var path = FileDialogForWindows.FileDialog("open VRM", "vrm");
#elif UNITY_EDITOR
            var path = UnityEditor.EditorUtility.OpenFilePanel("Open VRM", "", "vrm");
#else
            var path = Application.dataPath + "/default.vrm";
#endif
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            OnOpenLocalFile?.Invoke(path);
        }
    }
}