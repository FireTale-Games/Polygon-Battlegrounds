using System;
using TMPro;
using UnityEngine;

namespace FTS.UI.Map
{
    internal abstract class MapDropdownSetting : MapBaseSetting
    {
        [Header("Data")]
        [SerializeField] protected int _defaultDropdownValue;
        [SerializeField] protected string _defaultLabelValue;
        
        [Header("Components")]
        [SerializeField] protected TMP_Dropdown _dropdown;
        [SerializeField] protected TextMeshProUGUI _label;

        public override void Initialize(Action onChangeData) => 
            _dropdown.onValueChanged.AddListener(_ => onChangeData?.Invoke());

        public override void SetDefaultValues(bool isHost)
        {
            _dropdown.gameObject.SetActive(isHost);
            _label.gameObject.SetActive(!isHost);
            if (isHost) _dropdown.value = _defaultDropdownValue;
            else _label.text = _defaultLabelValue;
        }

        public abstract float GetDropdownValue();
        public abstract void SetLabelValue(float value);

        private void OnDestroy() => _dropdown.onValueChanged.RemoveAllListeners();
    }
}