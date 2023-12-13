using System;
using FTS.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    internal sealed class MapSettingsHostUi : MonoBehaviour
    {
        [SerializeField] private Sprite[] _trueFalse;
        [SerializeField] private TMP_Dropdown _aiDifficultyDropdown;
        [SerializeField] private TMP_Dropdown _resourcesDropDropdown;
        [SerializeField] private TMP_Dropdown _craftCostDropdown;
        [SerializeField] private TMP_Dropdown _upgradeCostDropdown;
        [SerializeField] private TMP_Dropdown _waveCooldownDropdown;
        [SerializeField] private Button _rarityButton;

        private bool _rarityValue = true;
        
        private Action<MapData> OnMapSettingChange;
        
        public void Initialize(Action<MapData> onMapSettingChange)
        {
            OnMapSettingChange = onMapSettingChange;
            
            _aiDifficultyDropdown.onValueChanged.AddListener(v => { OnAiDifficulty(v); ChangeMapData(); });
            _resourcesDropDropdown.onValueChanged.AddListener(v => { OnResourcesDrop(v); ChangeMapData(); });
            _craftCostDropdown.onValueChanged.AddListener(v => { OnCraftCost(v); ChangeMapData(); });
            _upgradeCostDropdown.onValueChanged.AddListener(v => { OnUpgradeCost(v); ChangeMapData(); });
            _waveCooldownDropdown.onValueChanged.AddListener(v => { OnWaveCooldown(v); ChangeMapData(); });
            _rarityButton.onClick.AddListener(() => { _rarityValue = !_rarityValue; ChangeMapData(); });
        }

        public void Show()
        {
            _aiDifficultyDropdown.value = 1;
            _resourcesDropDropdown.value = 2;
            _craftCostDropdown.value = 2;
            _upgradeCostDropdown.value = 2;
            _waveCooldownDropdown.value = 2;
            _rarityValue = true;
        }
        
        private void ChangeMapData()
        {
            MapData mapData = new(
                OnAiDifficulty(_aiDifficultyDropdown.value), 
                OnResourcesDrop(_resourcesDropDropdown.value),
                OnCraftCost(_craftCostDropdown.value),
                OnUpgradeCost(_upgradeCostDropdown.value),
                OnWaveCooldown(_waveCooldownDropdown.value),
                OnMonsterRarity(_rarityValue)
            );
                
            OnMapSettingChange?.Invoke(mapData);
        }
        
        private float OnAiDifficulty(int value) =>
            value switch
            {
                0 => 0.5f, 
                1 => 1.0f, 
                2 => 2.0f,
                _ => 1.0f
            };
        private float OnResourcesDrop(int value) =>
            value switch
            {
                0 => 0.5f,
                1 => 0.75f,
                2 => 1.0f,
                3 => 1.25f,
                4 => 1.5f,
                5 => 1.75f,
                6 => 2.0f,
                _ => 1.0f
            };
        private float OnCraftCost(int value) =>
            value switch
            {
                0 => 0.5f,
                1 => 0.75f,
                2 => 1.0f,
                3 => 1.25f,
                4 => 1.5f,
                5 => 1.75f,
                6 => 2.0f,
                _ => 1.0f
            };
        private float OnUpgradeCost(int value) =>
            value switch
            {
                0 => 0.5f,
                1 => 0.75f,
                2 => 1.0f,
                3 => 1.25f,
                4 => 1.5f,
                5 => 1.75f,
                6 => 2.0f,
                _ => 1.0f
            };
        private float OnWaveCooldown(int value) =>
            value switch
            {
                0 => 10.0f,
                1 => 15.0f,
                2 => 20.0f,
                3 => 25.0f,
                4 => 30.0f,
                _ => 20.0f
            };
        private bool OnMonsterRarity(bool value)
        {
            value = !value;
            _rarityButton.transform.GetChild(0).GetComponent<Image>().sprite = value ? _trueFalse[0] : _trueFalse[1];
            return value;
        }
            
    }
}