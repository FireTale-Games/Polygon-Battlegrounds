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
            _aiDifficultyDropdownMapSetting.SetLabelValue(mapData.r_aiDifficulty);
            _resourcesDropDropdownMapSetting.SetLabelValue(mapData.r_resourcesDrop);
            _craftCostDropdownMapSetting.SetLabelValue(mapData.r_craftCost);
            _upgradeCostDropdownMapSetting.SetLabelValue(mapData.r_upgradeCost);
            _waveCooldownDropdownMapSetting.SetLabelValue(mapData.r_waveCooldown);
            _monsterRarityCheckerMapSetting.SetSpriteValue(mapData.r_monsterRarity ? _trueFalseSprites[0] : _trueFalseSprites[1]);
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