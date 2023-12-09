using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FTS.Data;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using PlayerSettings = FTS.Data.PlayerSettings;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    internal sealed class MenuPlayManager : BaseManager
    {
        [SerializeField] private GameSettings _gameSettings = new();
        public IGameSettings GameSettings => _gameSettings;
        
        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }

        public const string KEY_PLAYER_NAME = "PlayerName";
        
        public Lobby GetJoinedLobby() => _joinedLobby;
        private Lobby _joinedLobby;
        
        private float heartbeatTimer;
        private float lobbyPollTimer;
        
        public event EventHandler<LobbyEventArgs> OnJoinedLobby;
        public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
        public class LobbyEventArgs : EventArgs {
            public Lobby lobby;
        }
        
        public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
        public class OnLobbyListChangedEventArgs : EventArgs {
            public List<Lobby> lobbyList;
        }
        
        public void SetGameType(GameType type) => _gameSettings.SetGameType(type);
        public void SetPlayerSettings(PlayerSettings playerSettings) => _gameSettings.SetPlayerSettings(playerSettings);
        public void SetLobbySettings(LobbySettings lobbySettings) => _gameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => _gameSettings.SetMapSettings(mapSettings);
        
        private void Update() {
            //HandleLobbyHeartbeat();
            //HandleLobbyPolling();
        }
        
        public bool IsLobbyHost() => 
            _joinedLobby != null && _joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
        
        private async void HandleLobbyHeartbeat()
        {
            if (!IsLobbyHost()) 
                return;
        
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer >= 0f) 
                return;
            
            const float heartbeatTimerMax = 15f;
            heartbeatTimer = heartbeatTimerMax;

            Debug.Log("Heartbeat");
            await LobbyService.Instance.SendHeartbeatPingAsync(_joinedLobby.Id);
        }

        private async void HandleLobbyPolling()
        {
            if (_joinedLobby == null) 
                return;
        
            lobbyPollTimer -= Time.deltaTime;
            if (lobbyPollTimer >= 0f) 
                return;
            
            const float lobbyPollTimerMax = 1.1f;
            lobbyPollTimer = lobbyPollTimerMax;

            _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);
            OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = _joinedLobby });
        }
        
        // AUTHENTICATION ----------------------------------------------------------
        #region AUTHENTICATION
        
        public async Task Authenticate()
        {
            int playerName = GameSettings.PlayerSettings.r_playerName;
            Dictionary<int, object> playerData = new DataLoader<Dictionary<int, object>, object>(playerName.ToString()).LoadData();
            await Authenticate(playerData[playerName] as string);
        }

        private async Task Authenticate(string playerName)
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            { 
                InitializationOptions options = new();
                options.SetProfile(playerName);
                await UnityServices.InitializeAsync(options);
                PlayerName = playerName;
            }

            AuthenticationService.Instance.SignedIn += () => Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerId = AuthenticationService.Instance.PlayerId;
                PlayerName = AuthenticationService.Instance.PlayerName;
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
                    IsPrivate = GameSettings.LobbySettings.r_lobbyType == LobbyType.Private,
                    Player = GetPlayer()
                };

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(GameSettings.LobbySettings.r_lobbyName, GameSettings.LobbySettings.r_playerNumber, lobbyOptions);
                _joinedLobby = lobby;
                
                Debug.Log("Created Lobby: " + GameSettings.LobbySettings.r_lobbyName + " with code " + _joinedLobby.LobbyCode);
                OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
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
                OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs { lobbyList = lobbyListQueryResponse.Results });
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

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }

        public async void JoinLobby(Lobby lobby) {
            Player player = GetPlayer();

            _joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions {
                Player = player
            });

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        }

        #endregion REFRESH_LOBBY
        
        private Player GetPlayer() =>
            new(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
                { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, PlayerName) },
            });
    }
}