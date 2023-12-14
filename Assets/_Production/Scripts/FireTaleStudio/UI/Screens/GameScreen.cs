using FTS.Managers;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [SerializeField] private MapSettingUi mapSettingUi;

        [Header("Host Components"), Space(2)] 
        [SerializeField] private RectTransform[] _mapRectTransform;
        [SerializeField] private MapSelectUi _mapSelectUi;
        [SerializeField] private LobbyCodeUi _lobbyCodeUi;
        
        [Header("Components"), Space(2)]
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform playerSingleTemplate;
        [SerializeField] private Transform _playerList;
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            lobbyManager.OnJoinedLobby += JoinLobby_Event;
            lobbyManager.OnJoinedLobbyUpdate += UpdateLobby_Event;
            _backButton.onClick.AddListener(OnLeaveLobby);
            
            // Host Data
            mapSettingUi.Initialize(lobbyManager.UpdateLobbyData);

            return;
            async void OnLeaveLobby() => await lobbyManager.LeaveLobby();
        }
        
        private void JoinLobby_Event(object sender, (Lobby lobby, bool isHost) e) =>
            JoinLobby(e);

        private void JoinLobby((Lobby lobby, bool isHost) e)
        {
            ClearLobby();

            foreach (Player player in e.lobby.Players) {
                Transform playerSingleTransform = Instantiate(playerSingleTemplate, _playerList);
                playerSingleTransform.gameObject.SetActive(true);
                LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
                lobbyPlayerSingleUI.UpdatePlayer(player);
            }
            
            mapSettingUi.SetDefaultValues(e.isHost);
            _mapSelectUi.SetDefaultValues(e.isHost, _mapRectTransform);
            _lobbyCodeUi.SetDefaultValues(e.isHost, e.isHost ? e.lobby.LobbyCode : string.Empty);
        }
        
        private void UpdateLobby_Event(object sender, Lobby lobby) => 
            UpdateLobby(lobby);

        private void UpdateLobby(Lobby lobby)
        {
            ClearLobby();

            foreach (Player player in lobby.Players) {
                Transform playerSingleTransform = Instantiate(playerSingleTemplate, _playerList);
                playerSingleTransform.gameObject.SetActive(true);
                LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
                lobbyPlayerSingleUI.UpdatePlayer(player);
            }
            
            mapSettingUi.UpdateMap(lobby);
        }
        
        private void ClearLobby() {
            foreach (Transform child in _playerList) {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
    }
}