using FTS.Managers;
using FTS.UI.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SettingsScreen : MenuScreenBase
    {
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _backButton;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is not SettingManager settingManager)
                return;
            
            _applyButton.onClick.AddListener(() => settingManager.SettingsApply(true));
            _backButton.onClick.AddListener(() => settingManager.SettingsApply(false));
            settingManager.SetInitialValues(GetComponentsInChildren<ISetting>());
        }
    }
}