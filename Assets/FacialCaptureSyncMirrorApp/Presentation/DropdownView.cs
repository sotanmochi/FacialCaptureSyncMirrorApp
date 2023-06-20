using System;
using UnityEngine;
using UnityEngine.UI;
using Dropdown = TMPro.TMP_Dropdown;

namespace FacialCaptureSync.MirrorApp
{
    public sealed class DropdownView : MonoBehaviour
    {
        [SerializeField] string _dropdownText;
        [SerializeField] Dropdown _dropdown;

        public event Action<(int Index, string Value)> OnSelected;

        private string[] _items = Array.Empty<string>();

        private void Awake()
        {
            _dropdown.onValueChanged.AddListener(OnValueChangedEventHandler);
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveAllListeners();
        }

        private void OnValueChangedEventHandler(int selectedIndex)
        {
            if (selectedIndex < 1) { return; }

            var index = selectedIndex - 1;
            var value = _items[index];

            OnSelected?.Invoke((index, value));
        }

        public void SetDrowdownText(string value)
        {
            _dropdownText = value;
        }

        public void UpdateDropdownOptions(string[] items)
        {
            _items = items;

            _dropdown.ClearOptions();
            _dropdown.RefreshShownValue();
            _dropdown.options.Add(new Dropdown.OptionData { text = _dropdownText });
            
            foreach (var item in items)
            {
                _dropdown.options.Add(new Dropdown.OptionData { text = item });
            }
            
            _dropdown.RefreshShownValue();
        }
    }
}