using System;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class PlayerUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private Button _kickPlayerButton;

        public void UpdatePlayer(Player player, Action<string> onKickPlayer)
        {
            _playerNameText.text = player.Data["PlayerName"].Value;

            if (onKickPlayer == null)
                return;
            
            _kickPlayerButton.gameObject.SetActive(true);
            _kickPlayerButton.onClick.AddListener(() => onKickPlayer.Invoke(player.Id));
        }
        
        private void OnDestroy() => _kickPlayerButton.onClick.RemoveAllListeners();
    }
}