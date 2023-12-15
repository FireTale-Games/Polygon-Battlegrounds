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
        [SerializeField] private Transform playerSingleTemplate;
        [SerializeField] private Transform _playerList;
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            lobbyManager.OnJoinedLobby += JoinLobby_Event;
            lobbyManager.OnJoinedLobbyUpdate += UpdateLobby_Event;
            _backButton.onClick.AddListener(OnLeaveLobby);
            
            // Host Data
            _mapSettingUi.Initialize(mapData => { lobbyManager.SetMapDataSettings(mapData); lobbyManager.UpdateLobbyData(); });
            _mapSelectionUi.Initialize(mapId => { lobbyManager.SetMapId(mapId); lobbyManager.UpdateLobbyData(); });

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
                PlayerUi playerUi = playerSingleTransform.GetComponent<PlayerUi>();
                playerUi.UpdatePlayer(player);
            }
            
            _mapSettingUi.SetDefaultValues(e.isHost);
            _mapSelectionUi.SetDefaultValues(e.isHost, ItemDatabase.GetAllOfType<GameMap>());
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
                PlayerUi playerUi = playerSingleTransform.GetComponent<PlayerUi>();
                playerUi.UpdatePlayer(player);
            }
            
            MapSettings mapSetting = JsonConvert.DeserializeObject<MapSettings>(lobby.Data["MapData"].Value);
            _mapSettingUi.UpdateMap(mapSetting.r_mapData);
            _mapDisplayUi.UpdateMap(mapSetting.r_mapId);
        }
        
        private void ClearLobby() {
            foreach (Transform child in _playerList) {
                if (child == playerSingleTemplate) continue;
                Destroy(child.gameObject);
            }
        }
    }
}