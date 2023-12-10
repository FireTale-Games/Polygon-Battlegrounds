using FTS.Managers;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [SerializeField] private Transform playerSingleTemplate;
        [SerializeField] private Transform _playerList;
        
        protected override void BindToLobbyManager(Managers.LobbyManager lobbyManager)
        {
            lobbyManager.OnJoinedLobby += UpdateLobby_Event;
            lobbyManager.OnJoinedLobbyUpdate += UpdateLobby_Event;

        }

        private void UpdateLobby_Event(object sender, Managers.LobbyManager.LobbyEventArgs e) => 
            UpdateLobby(e.lobby);

        private void UpdateLobby(Lobby lobby) {
            ClearLobby();

            foreach (Player player in lobby.Players) {
                Transform playerSingleTransform = Instantiate(playerSingleTemplate, _playerList);
                playerSingleTransform.gameObject.SetActive(true);
                LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
                lobbyPlayerSingleUI.UpdatePlayer(player);
            }
        }
        
        private void ClearLobby() {
            foreach (Transform child in _playerList) {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
    }
}