using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private GameSettings _gameSettings = new();
        public IGameSettings GameSettings => _gameSettings;

        private string _playerId;
        private string _playerName;
        private Lobby _joinedLobby;
        
        private const float _heartbeatTimerDelay = 25.0f;
        private const float k_poolingTimer = 1.05f;

        private readonly CountdownTimer _heartbeatTimer = new(_heartbeatTimerDelay);
        private readonly CountdownTimer _poolingTimer = new(k_poolingTimer);
        
        public event EventHandler<(Lobby, bool)> OnJoinedLobby;
        public event EventHandler<(Lobby, bool)> OnJoinedLobbyUpdate;
        public event EventHandler<Lobby> OnLeaveLobby;
        public event EventHandler<Lobby[]> OnLobbyListChanged;

        public void SetGameType(GameType type) => _gameSettings.SetGameType(type);
        public void SetPlayerSettings(PlayerSettings playerSettings) => _gameSettings.SetPlayerSettings(playerSettings);
        public void SetLobbySettings(LobbySettings lobbySettings) => _gameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => _gameSettings.SetMapSettings(mapSettings);
        public void SetMapDataSettings(MapData mapData) => _gameSettings.SetMapDataSettings(mapData);
        public void SetMapId(int mapId) => _gameSettings.SetMapId(mapId); 

        private void Awake()
        {
            _heartbeatTimer.OnTimeStop += HandleLobbyHeartbeat;
            _poolingTimer.OnTimeStop += HandleLobbyPolling;
        }

        private void Update() {
            _heartbeatTimer.Tick(Time.deltaTime);
            _poolingTimer.Tick(Time.deltaTime);
        }
        
        private async void HandleLobbyHeartbeat()
        {
            if (!IsLobbyHost())
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

        private async void HandleLobbyPolling()
        {
            try
            {
                _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);
                if (!IsPlayerInLobby()) {
                    Debug.Log("Kicked from Lobby!");
                    OnLeaveLobby?.Invoke(this, _joinedLobby);
                    _joinedLobby = null;
                    _poolingTimer.Stop();
                    return;
                }
                
                OnJoinedLobbyUpdate?.Invoke(this, (_joinedLobby, IsLobbyHost()));
                _poolingTimer.Start();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
                _poolingTimer.Pause();
                OnLeaveLobby?.Invoke(this, _joinedLobby);
            }
        }
        
        // AUTHENTICATION ----------------------------------------------------------
        #region AUTHENTICATION
        
        public async Task Authenticate()
        {
            int playerName = GameSettings.PlayerSettings.r_playerName;
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
                        {"Password", new DataObject(DataObject.VisibilityOptions.Public, GameSettings.LobbySettings.r_lobbyPassword)},
                        {"MapData", new DataObject(DataObject.VisibilityOptions.Member, JsonConvert.SerializeObject(GameSettings.MapSettings))}
                    },
                    Player = GetPlayer()
                };

                Debug.Log(lobbyOptions.Player.Data["PlayerName"].Value);
                
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(GameSettings.LobbySettings.r_lobbyName, GameSettings.LobbySettings.r_playerNumber, lobbyOptions);
                _joinedLobby = lobby;
                _heartbeatTimer.Start();
                _poolingTimer.Start();
                
                Debug.Log("Created Lobby: " + GameSettings.LobbySettings.r_lobbyName + " with code " + _joinedLobby.LobbyCode);
                OnJoinedLobby?.Invoke(this, (lobby, true));
                Debug.Log("Created Lobby " + lobby.Name);
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
            OnJoinedLobby?.Invoke(this, (lobby, false));
        }

        public async void JoinLobby(Lobby lobby) {
            try
            {
                Player player = GetPlayer();

                _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions
                {
                    Player = player
                });

                _poolingTimer.Start();
                OnJoinedLobby?.Invoke(this, (lobby, false));
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
                if (IsLobbyHost())
                {
                    _heartbeatTimer.Pause();
                    _poolingTimer.Pause();
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
            if (!IsLobbyHost()) 
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
        
        // UPDATE LOBBY -------------------------------------------------------------------
        #region UPDATE_LOBBY

        public async void UpdateLobbyData()
        {
            try
            {
                Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(_joinedLobby.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        { "MapData", new DataObject(DataObject.VisibilityOptions.Member, JsonConvert.SerializeObject(_gameSettings.MapSettings)) }
                    }
                });

                _joinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        #endregion
        
        private Player GetPlayer() =>
            new(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
                { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, _playerName) },
            });
        
        private bool IsLobbyHost() => _joinedLobby != null && _joinedLobby.HostId == _playerId;
        
        private bool IsPlayerInLobby() =>
            _joinedLobby is { Players: not null } && _joinedLobby.Players.Any(player => player.Id == _playerId);
    }
}