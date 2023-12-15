using System;
using FTS.Data;
using FTS.UI.Map;
using UnityEngine;

namespace FTS.UI
{
    internal sealed class MapSettingUi : MonoBehaviour
    {
        [SerializeField] private Sprite[] _trueFalseSprites;
        
        [SerializeField] private AiDifficultyDropdownMapSetting _aiDifficultyDropdownMapSetting;
        [SerializeField] private ResourcesDropDropdownMapSetting _resourcesDropDropdownMapSetting;
        [SerializeField] private CraftCostDropdownMapSetting _craftCostDropdownMapSetting;
        [SerializeField] private UpgradeCostDropdownMapSetting _upgradeCostDropdownMapSetting;
        [SerializeField] private WaveCooldownDropdownMapSetting _waveCooldownDropdownMapSetting;
        [SerializeField] private MonsterRarityCheckerMapSetting _monsterRarityCheckerMapSetting;

        public void SetDefaultValues(bool isLobbyHost)
        {
            IMapSetting[] mapSettings = GetComponentsInChildren<IMapSetting>();
            foreach (IMapSetting mapSetting in mapSettings)
                mapSetting.SetDefaultValues(isLobbyHost);
        }

        public void UpdateMap(MapData mapData)
        {
            _aiDifficultyDropdownMapSetting.SetValue(mapData.r_aiDifficulty);
            _resourcesDropDropdownMapSetting.SetValue(mapData.r_resourcesDrop);
            _craftCostDropdownMapSetting.SetValue(mapData.r_craftCost);
            _upgradeCostDropdownMapSetting.SetValue(mapData.r_upgradeCost);
            _waveCooldownDropdownMapSetting.SetValue(mapData.r_waveCooldown);
            _monsterRarityCheckerMapSetting.SetValue(mapData.r_monsterRarity ? _trueFalseSprites[0] : _trueFalseSprites[1]);
        }

        public void Initialize(Action<MapData> updateLobbyData)
        {
            IMapSetting[] mapSettings = GetComponentsInChildren<IMapSetting>();
            foreach (IMapSetting mapSetting in mapSettings)
                mapSetting.Initialize(ChangeMapData);

            return;

            void ChangeMapData()
            {
                MapData mapData = new(
                    _aiDifficultyDropdownMapSetting.GetDropdownValue(),
                    _resourcesDropDropdownMapSetting.GetDropdownValue(), 
                    _craftCostDropdownMapSetting.GetDropdownValue(), 
                    _upgradeCostDropdownMapSetting.GetDropdownValue(), 
                    _waveCooldownDropdownMapSetting.GetDropdownValue(),
                    _monsterRarityCheckerMapSetting.GetCheckerValue()
                );

                updateLobbyData?.Invoke(mapData);
            }
        }
    }
}