using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class PlayerUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private Button kickPlayerButton;

        public void SetKickPlayerButtonVisible(bool visible)
        {
            kickPlayerButton.gameObject.SetActive(visible);
        }

        public void UpdatePlayer(Player player)
        {
            playerNameText.text = player.Data["PlayerName"].Value;
            kickPlayerButton.onClick.AddListener(() => KickPlayer(player.Id));
        }

        private void KickPlayer(string playerId)
        {
            //LobbyManager.Instance.KickPlayer(playerId);
        }
    }
}