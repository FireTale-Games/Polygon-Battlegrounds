using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class PlayerUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private Button _kickPlayerButton;

        public void UpdatePlayer(string playerName, string playerId, Action<string> onKickPlayer)
        {
            _playerNameText.text = playerName;

            if (onKickPlayer == null)
                return;
            
            _kickPlayerButton.gameObject.SetActive(true);
            _kickPlayerButton.onClick.AddListener(() => onKickPlayer.Invoke(playerId));
        }
        
        private void OnDestroy() => _kickPlayerButton.onClick.RemoveAllListeners();
    }
}