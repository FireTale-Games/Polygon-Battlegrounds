using FTS.Managers;
using FTS.Tools.ExtensionMethods;
using FTS.Tools.ScriptableEvents;
using FTS.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SettingsScreen : MenuScreenBase
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _backButton;
        
        private EventInvoker<bool> OnSettingsApply => _onSettingsApply ??= ExtensionMethods.LoadEventObject<bool>(nameof(OnSettingsApply));
        private EventInvoker<bool> _onSettingsApply;
        
        private void Start()
        {
            _applyButton.onClick.AddListener(() => OnSettingsApply.Null()?.Raise(true));
            _backButton.onClick.AddListener(() => OnSettingsApply.Null()?.Raise(false));
            FindObjectOfType<SettingManager>().SetInitialValues(GetComponentsInChildren<ISetting>());
        }
    }
}