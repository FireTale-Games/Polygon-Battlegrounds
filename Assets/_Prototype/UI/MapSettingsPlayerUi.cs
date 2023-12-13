using FTS.Data;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    public class MapSettingsPlayerUi : MonoBehaviour
    {
        [SerializeField] private Sprite[] _trueFalse;
        [SerializeField] private TextMeshProUGUI _aiDifficultyText;
        [SerializeField] private TextMeshProUGUI _resourcesDropText;
        [SerializeField] private TextMeshProUGUI _craftCostText;
        [SerializeField] private TextMeshProUGUI _upgradeCostText;
        [SerializeField] private TextMeshProUGUI _waveCooldownText;
        [SerializeField] private Image _monsterRarityImage;

        public void UpdateMap(Lobby lobby)
        {
            if (!isActiveAndEnabled)
                return;
            MapData mapData = JsonConvert.DeserializeObject<MapData>(lobby.Data["MapData"].Value);
            _aiDifficultyText.text = AiDifficulty(mapData.r_aiDifficulty);
            _resourcesDropText.text = $"{mapData.r_resourcesDrop * 100.0f}%";
            _craftCostText.text = $"{mapData.r_craftCost * 100.0f}%";
            _upgradeCostText.text = $"{mapData.r_upgradeCost * 100.0f}%";
            _waveCooldownText.text = $"{mapData.r_waveCooldown}s";
            _monsterRarityImage.sprite = mapData.r_monsterRarity ? _trueFalse[0] : _trueFalse[1];
        }

        private string AiDifficulty(float value) =>
            value switch
            {
                0.5f => "Easy", 
                1.0f => "Medium", 
                2.0f => "Hard",
                _ => "Normal"
            };
    }
}