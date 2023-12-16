using System;
using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using FTS.UI.GameLobby;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [SerializeField] private MapSettingUi _mapSettingUi;
        [SerializeField] private MapDisplayUi _mapDisplayUi;
        
        [Header("Host Components"), Space(2)] 
        [SerializeField] private MapSelectionUi _mapSelectionUi;
        [SerializeField] private LobbyCodeUi _lobbyCodeUi;
        
        [Header("Components"), Space(2)]
        [SerializeField] private Button _backButton;
        [SerializeField] private PlayerUi _playerUi;
        [SerializeField] private Transform _playerList;

        private Action<string> _onPlayerKick;
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            lobbyManager.LobbyNetworkUpdate.OnLobbyPlayersUpdate += JoinLobby;
            lobbyManager.LobbyNetworkUpdate.OnSettingsUpdate += UpdateLobby;

            _backButton.onClick.AddListener(OnLeaveLobby);
            _onPlayerKick = OnKickFromLobby;
            
            // Host Data
            _mapSettingUi.Initialize(lobbyManager.SetMapDataSettings);
            _mapSelectionUi.Initialize(lobbyManager.SetMapId);
            
            return;
            async void OnLeaveLobby() => await lobbyManager.LeaveLobby();
            async void OnKickFromLobby(string id) => await lobbyManager.KickPlayer(id);
        }

        //private void LeaveLobby_Event(object sender, Lobby e)
        //{
        //    ClearLobby();
        //    _backButton.onClick?.Invoke();
        //}

        private void JoinLobby(object sender, LobbyRef lobbyRef)
        {
            ClearLobby();

            for (int i = 0; i < lobbyRef.r_playerRefs.Length; i++)
                Instantiate(_playerUi, _playerList).UpdatePlayer(lobbyRef.r_playerRefs[i].r_playerName, lobbyRef.r_playerRefs[i].r_playerId,
                    lobbyRef.r_isHost && i != 0 ? _onPlayerKick : null);

            _mapSettingUi.SetDefaultValues(lobbyRef.r_isHost);
            _mapSelectionUi.SetDefaultValues(lobbyRef.r_isHost, ItemDatabase.GetAllOfType<GameMap>());
            _lobbyCodeUi.SetDefaultValues(lobbyRef.r_isHost, lobbyRef.r_isHost ? lobbyRef.r_lobbyCode : string.Empty);
        }
        
        private void UpdateLobby(object sender, MapSettings mapSettings)
        {
            _mapSettingUi.UpdateMap(mapSettings.r_mapData);
            _mapDisplayUi.UpdateMap(mapSettings.r_mapId);
        }
        
        private void ClearLobby()
        {
            foreach (PlayerUi playerUi in _playerList.GetComponentsInChildren<PlayerUi>())
                Destroy(playerUi.gameObject);
        }
    }
}