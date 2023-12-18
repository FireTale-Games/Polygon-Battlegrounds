using FTS.Data;
using FTS.Data.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class MapDisplayUi : MonoBehaviour
    {
        [SerializeField] private Image _mapImage;
        [SerializeField] private Button _mapButton;
        [SerializeField] private TextMeshProUGUI _mapLabel;

        public void UpdateMap(int mapId)
        {
            GameMap gameMap = ItemDatabase.Get<GameMap>(mapId);
            _mapImage.sprite = gameMap.Sprite;
            _mapLabel.text = gameMap.DisplayName;
        }
        
        private void OnDestroy() => _mapButton.onClick.RemoveAllListeners();
    }
}