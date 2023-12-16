using System;
using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using FTS.UI.GameLobby;
using Newtonsoft.Json;
using Unity.Services.Lobbies.Models;
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
            lobbyManager.OnJoinedLobby += JoinLobby_Event;
            lobbyManager.OnJoinedLobbyUpdate += UpdateLobby_Event;
            lobbyManager.OnLeaveLobby += LeaveLobby_Event;
            _backButton.onClick.AddListener(OnLeaveLobby);
            _onPlayerKick = OnKickFromLobby;
            
            // Host Data
            _mapSettingUi.Initialize(mapData => { lobbyManager.SetMapDataSettings(mapData); lobbyManager.UpdateLobbyData(); });
            _mapSelectionUi.Initialize(mapId => { lobbyManager.SetMapId(mapId); lobbyManager.UpdateLobbyData(); });
            
            return;
            async void OnLeaveLobby() => await lobbyManager.LeaveLobby();
            async void OnKickFromLobby(string id) => await lobbyManager.KickPlayer(id);
        }

        private void LeaveLobby_Event(object sender, Lobby e)
        {
            ClearLobby();
            _backButton.onClick?.Invoke();
        }

        private void JoinLobby_Event(object sender, (Lobby lobby, bool isHost) e) =>
            JoinLobby(e);

        private void JoinLobby((Lobby lobby, bool isHost) e)
        {
            ClearLobby();
            
            foreach (Player player in e.lobby.Players) 
                Instantiate(_playerUi, _playerList).UpdatePlayer(player, e.isHost && player.Id != e.lobby.HostId ? _onPlayerKick : null);
            
            _mapSettingUi.SetDefaultValues(e.isHost);
            _mapSelectionUi.SetDefaultValues(e.isHost, ItemDatabase.GetAllOfType<GameMap>());
            _lobbyCodeUi.SetDefaultValues(e.isHost, e.isHost ? e.lobby.LobbyCode : string.Empty);
        }
        
        private void UpdateLobby_Event(object sender, (Lobby lobby, bool isHost) e) =>
            UpdateLobby(e);

        private void UpdateLobby((Lobby lobby, bool isHost) e)
        {
            ClearLobby();

            foreach (Player player in e.lobby.Players) 
                Instantiate(_playerUi, _playerList).UpdatePlayer(player, e.isHost && player.Id != e.lobby.HostId ? _onPlayerKick : null);            
            
            MapSettings mapSetting = JsonConvert.DeserializeObject<MapSettings>(e.lobby.Data["MapData"].Value);
            _mapSettingUi.UpdateMap(mapSetting.r_mapData);
            _mapDisplayUi.UpdateMap(mapSetting.r_mapId);
        }
        
        private void ClearLobby()
        {
            foreach (PlayerUi playerUi in _playerList.GetComponentsInChildren<PlayerUi>())
                Destroy(playerUi.gameObject);
        }
    }
}