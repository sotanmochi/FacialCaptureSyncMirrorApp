using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class CaptureSourceConnectionView : MonoBehaviour
    {
        [SerializeField] Button _connect;
        [SerializeField] TMP_InputField _ipAddress;
        [SerializeField] DropdownView _captureSourceTypeDropdown;

        public event Action<CaptureSourcePropeties> OnClickConnect;

        private (int Index, string Value) _selectedCaptureSourceType;

        void Awake()
        {
            _connect.onClick.AddListener(OnClickConnectEventHandler);
            _captureSourceTypeDropdown.OnSelected += OnSelectedEventHandler;
        }

        void OnDestroy()
        {
            _connect.onClick.RemoveAllListeners();
            _captureSourceTypeDropdown.OnSelected -= OnSelectedEventHandler;
        }

        private void OnClickConnectEventHandler()
        {
            var propeties = new CaptureSourcePropeties()
            {
                CaptureSourceDeviceIpAddress = _ipAddress.text,
                CaptureSourceTypeName = _selectedCaptureSourceType.Value,
                CaptureSourceTypeIndex = _selectedCaptureSourceType.Index,
            };
            OnClickConnect?.Invoke(propeties);
        }

        private void OnSelectedEventHandler((int Index, string Value) selectedItem)
        {
            _selectedCaptureSourceType = selectedItem;
        }

        public void UpdateDropdownOptions(string[] items)
        {
            _captureSourceTypeDropdown.UpdateDropdownOptions(items);
        }
    }

    public class CaptureSourcePropeties
    {
        public string CaptureSourceDeviceIpAddress;
        public string CaptureSourceTypeName;
        public int CaptureSourceTypeIndex;
    }
}