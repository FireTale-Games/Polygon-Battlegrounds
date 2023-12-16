using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FTS.Data;
using FTS.Tools.Utilities;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using PlayerSettings = FTS.Data.PlayerSettings;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    internal sealed class LobbyManager : BaseManager
    {
        [SerializeField] private LobbyNetworkManager _lobbyNetworkManager;
        [SerializeField] private GameSettings _gameSettings;

        private bool IsLobbyHost => _joinedLobby != null && _joinedLobby.HostId == _playerId;
        public GameType GameType => _gameSettings.GameType;
        public ILobbyNetworkUpdate LobbyNetworkUpdate => _lobbyNetworkManager;
        private string _playerId;
        private string _playerName;
        private Lobby _joinedLobby;
        
        private const float _heartbeatTimerDelay = 25.0f;
        private readonly CountdownTimer _heartbeatTimer = new(_heartbeatTimerDelay);
        
        public event EventHandler<Lobby[]> OnLobbyListChanged;

        public void SetGameType(GameType type) => _gameSettings.SetGameType(type);
        public void SetPlayerSettings(PlayerSettings playerSettings) => _gameSettings.SetPlayerSettings(playerSettings);
        public void SetLobbySettings(LobbySettings lobbySettings) => _gameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => _gameSettings.SetMapSettings(mapSettings);

        public void SetMapDataSettings(MapData mapData)
        {
            _gameSettings.SetMapDataSettings(mapData);
            _lobbyNetworkManager.UpdateLobby(_gameSettings.MapSettings);
        }

        public void SetMapId(int mapId)
        {
            _gameSettings.SetMapId(mapId);
            _lobbyNetworkManager.UpdateLobby(_gameSettings.MapSettings);
        }

        private void Awake() => _heartbeatTimer.OnTimeStop += HandleLobbyHeartbeat;
        private void Update() => _heartbeatTimer.Tick(Time.deltaTime);

        private async void HandleLobbyHeartbeat()
        {
            if (!IsLobbyHost)
            {
                _heartbeatTimer.Pause();
                return;
            }
            
            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
                _heartbeatTimer.Start();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                _heartbeatTimer.Pause();
            }
        }
        
        //private void HandleLobbyPolling()
        //{
            //_poolingTimer.Start();
            //_lobbyNetworkManager.Display(_joinedLobby.Id);

            //try
            //{
            //    _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);
            //    if (!IsPlayerInLobby()) {
            //        Debug.Log("Kicked from Lobby!");
            //        OnLeaveLobby?.Invoke(this, _joinedLobby);
            //        _joinedLobby = null;
            //        _poolingTimer.Stop();
            //        return;
            //    }
            //    
            //    OnJoinedLobbyUpdate?.Invoke(this, (_joinedLobby, IsLobbyHost()));
            //    _poolingTimer.Start();
            //}
            //catch (LobbyServiceException e)
            //{
            //    Debug.Log(e);
            //    _poolingTimer.Pause();
            //    OnLeaveLobby?.Invoke(this, _joinedLobby);
            //}
        //}
        
        // AUTHENTICATION ----------------------------------------------------------
        #region AUTHENTICATION
        
        public async Task Authenticate()
        {
            int playerName = _gameSettings.PlayerSettings.r_playerName;
            Dictionary<int, object> playerData = new DataLoader<Dictionary<int, object>, object>(playerName.ToString()).LoadData();
            await Authenticate(playerData[playerName].ToString());
        }

        private async Task Authenticate(string playerName)
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                InitializationOptions options = new();
                options.SetProfile(playerName);
                await UnityServices.InitializeAsync(options);
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                _playerId = AuthenticationService.Instance.PlayerId;
                _playerName = playerName;
            }
        }
        
        #endregion AUTHENTICATION
        
        // CREATE LOBBY ------------------------------------------------------------
        #region CREATE_LOBBY
        
        public async Task CreateLobby()
        {
            try
            {
                CreateLobbyOptions lobbyOptions = new()
                {
                    IsPrivate = false,
                    Data = new Dictionary<string, DataObject>
                    {
                        {"Password", new DataObject(DataObject.VisibilityOptions.Public, _gameSettings.LobbySettings.r_lobbyPassword)},
                        {"MapData", new DataObject(DataObject.VisibilityOptions.Member, JsonConvert.SerializeObject(_gameSettings.MapSettings))}
                    },
                    Player = GetPlayer()
                };
                
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_gameSettings.LobbySettings.r_lobbyName, _gameSettings.LobbySettings.r_playerNumber, lobbyOptions);
                _joinedLobby = lobby;
                _heartbeatTimer.Start();
                Debug.Log("Created Lobby: " + _gameSettings.LobbySettings.r_lobbyName + " with code " + _joinedLobby.LobbyCode);
                _lobbyNetworkManager.StartHost(_joinedLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Failed to create lobby: " + e.Message);
            }
        }
        
        
        #endregion CREATE_LOBBY
        
        // REFRESH LOBBIES ------------------------------------------------------------
        #region REFRESH_LOBBY

        public async void RefreshLobbyList() {
            try {
                QueryLobbiesOptions options = new();
                options.Count = 25;
            
                options.Filters = new List<QueryFilter> {
                    new(field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0")
                };
            
                options.Order = new List<QueryOrder> {
                    new(asc: false,
                        field: QueryOrder.FieldOptions.Created)
                };

                QueryResponse lobbyListQueryResponse = await Lobbies.Instance.QueryLobbiesAsync();
                OnLobbyListChanged?.Invoke(this, lobbyListQueryResponse.Results.ToArray());
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }
        
        #endregion REFRESH_LOBBY
        
        // JOIN LOBBY ------------------------------------------------------------
        #region JOIN_LOBBY
    
        public async void JoinLobbyByCode(string lobbyCode) {
            Player player = GetPlayer();

            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions {
                Player = player
            });

            _joinedLobby = lobby;
            _lobbyNetworkManager.StartClient(_joinedLobby);
        }

        public async void JoinLobby(Lobby lobby) {
            try
            {
                Player player = GetPlayer();

                _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
                {
                    Player = player
                });

                _lobbyNetworkManager.StartClient(_joinedLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        #endregion JOIN_LOBBY
        
        // DELETE LOBBY --------------------------------------------------------
        #region DELETE_LOBBY

        public async Task LeaveLobby()
        {
            try
            {
                if (IsLobbyHost)
                {
                    _heartbeatTimer.Pause();
                    //_poolingTimer.Pause();
                    for (int i = _joinedLobby.Players.Count - 1; i >= 0; i--)
                        await Lobbies.Instance.RemovePlayerAsync(_joinedLobby.Id, _joinedLobby.Players[i].Id);
                    await Lobbies.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                }
                else
                    await Lobbies.Instance.RemovePlayerAsync(_joinedLobby.Id, _playerId);
                
                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        
        public async Task KickPlayer(string playerId)
        {
            if (!IsLobbyHost) 
                return;
            
            try 
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, playerId);
            } 
            catch (LobbyServiceException e) 
            {
                Debug.Log(e);
            }
        }
        
        #endregion : DELETE_LOBBY
        
        private Player GetPlayer() =>
            new(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, _playerName) },
            });
    }
}