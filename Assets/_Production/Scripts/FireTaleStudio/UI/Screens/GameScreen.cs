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
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform _playerList;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is MultiplayerManager multiplayerManager)
                _backButton.onClick.AddListener(() => multiplayerManager.SetNetworkConnection(false));

            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            menuPlayManager.OnJoinedLobby += UpdateLobby_Event;
            menuPlayManager.OnJoinedLobbyUpdate += UpdateLobby_Event;

        }

        private void UpdateLobby_Event(object sender, MenuPlayManager.LobbyEventArgs e) => 
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