using System;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class LobbyListSingleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lobbyNameText;
        [SerializeField] private TextMeshProUGUI playersText;
        [SerializeField] private Button _joinButton;

        public void UpdateLobby(Action<Lobby> onLobbyJoin, Lobby lobby)
        {
            _joinButton.onClick.AddListener(() => onLobbyJoin?.Invoke(lobby));
            lobbyNameText.text = lobby.Name;
            playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        }

        private void OnDestroy() =>
            _joinButton.onClick.RemoveAllListeners();
    }
}