using System;
using FTS.Data.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class SelectMapUi : MonoBehaviour
    {
        [SerializeField] private Image _mapImage;
        [SerializeField] private Button _mapButton;
        [SerializeField] private TextMeshProUGUI _mapLabel;
        
        public void Initialize(GameMap gameMap, Action<int> updateLobbyData)
        {
            _mapImage.sprite = gameMap.Sprite;
            _mapLabel.text = gameMap.DisplayName;
            _mapButton.onClick.AddListener(() => updateLobbyData?.Invoke(gameMap.Id));
        }

        private void OnDestroy() => _mapButton.onClick.RemoveAllListeners();
    }
}