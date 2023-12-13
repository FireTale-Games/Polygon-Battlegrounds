using System;
using FTS.Managers;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [Header("Host Data")]
        [SerializeField] private GameObject _mapSelection;
        [SerializeField] private MapSettingsHostUi _mapSettingsHostUi;
        
        [Header("Player Data")]
        [SerializeField] private MapSettingsPlayerUi _mapSettingsPlayerUi;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Transform playerSingleTemplate;
        [SerializeField] private Transform _playerList;

        private Action OnOpenPanel;
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            lobbyManager.OnJoinedLobby += UpdateLobby_Event;
            lobbyManager.OnJoinedLobbyUpdate += UpdateLobby_Event;
            _backButton.onClick.AddListener(OnLeaveLobby);
            
            // Host Data
            _mapSettingsHostUi.Initialize(lobbyManager.UpdateLobbyData);

            // General Actions
            OnOpenPanel = OnOpen;
            
            return;
            async void OnLeaveLobby() => await lobbyManager.LeaveLobby();
            void OnOpen()
            {
                _mapSelection.SetActive(lobbyManager.IsLobbyHost());
                _mapSettingsHostUi.gameObject.SetActive(lobbyManager.IsLobbyHost());
                _mapSettingsPlayerUi.gameObject.SetActive(!lobbyManager.IsLobbyHost());
            }
        }

        private void UpdateLobby_Event(object sender, Lobby lobby)
        {
            UpdateLobby(lobby);
            OnOpenPanel?.Invoke();
        }

        private void UpdateLobby(Lobby lobby)
        {
            ClearLobby();

            foreach (Player player in lobby.Players) {
                Transform playerSingleTransform = Instantiate(playerSingleTemplate, _playerList);
                playerSingleTransform.gameObject.SetActive(true);
                LobbyPlayerSingleUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<LobbyPlayerSingleUI>();
                lobbyPlayerSingleUI.UpdatePlayer(player);
            }
            
            _mapSettingsPlayerUi.UpdateMap(lobby);
        }
        
        public override void Show(float? speed = null)
        {
            base.Show(speed);
            _mapSettingsHostUi.Show();
        }

        private void ClearLobby() {
            foreach (Transform child in _playerList) {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
    }
}