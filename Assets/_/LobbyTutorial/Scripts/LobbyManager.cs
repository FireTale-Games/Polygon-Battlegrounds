using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : MonoBehaviour {
    
    public static LobbyManager Instance { get; private set; }
    
    public const string KEY_PLAYER_NAME = "PlayerName";

    public event EventHandler OnLeftLobby;

    public event EventHandler<LobbyEventArgs> OnJoinedLobby;
    public event EventHandler<LobbyEventArgs> OnJoinedLobbyUpdate;
    public event EventHandler<LobbyEventArgs> OnKickedFromLobby;
    public class LobbyEventArgs : EventArgs {
        public Lobby lobby;
    }

    public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs : EventArgs {
        public List<Lobby> lobbyList;
    }

    private float heartbeatTimer;
    private float lobbyPollTimer;
    private float refreshLobbyListTimer = 5f;
    private Lobby joinedLobby;
    private string playerName;


    private void Awake() {
        Instance = this;
    }

    private void Update() {
        //HandleRefreshLobbyList(); // Disabled Auto Refresh for testing with multiple builds
        HandleLobbyHeartbeat();
        HandleLobbyPolling();
    }

    public async void Authenticate(string pName) {
        playerName = pName;
        InitializationOptions initializationOptions = new();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
            RefreshLobbyList();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void HandleRefreshLobbyList()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized || !AuthenticationService.Instance.IsSignedIn) 
            return;
        
        refreshLobbyListTimer -= Time.deltaTime;
        if (refreshLobbyListTimer >= 0f) 
            return;
            
        const float refreshLobbyListTimerMax = 5f;
        refreshLobbyListTimer = refreshLobbyListTimerMax;

        RefreshLobbyList();
    }

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
        await LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
    }

    private async void HandleLobbyPolling()
    {
        if (joinedLobby == null) 
            return;
        
        lobbyPollTimer -= Time.deltaTime;
        if (lobbyPollTimer >= 0f) 
            return;
            
        const float lobbyPollTimerMax = 1.1f;
        lobbyPollTimer = lobbyPollTimerMax;

        joinedLobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);

        OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });

        if (IsPlayerInLobby()) 
            return;
                
        Debug.Log("Kicked from Lobby!");
                
        OnKickedFromLobby?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
        joinedLobby = null;
    }

    public Lobby GetJoinedLobby() => joinedLobby;

    public bool IsLobbyHost() => 
        joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;

    private bool IsPlayerInLobby() => 
        joinedLobby is { Players: not null } && joinedLobby.Players.Any(player => player.Id == AuthenticationService.Instance.PlayerId);

    private Player GetPlayer() =>
        new(AuthenticationService.Instance.PlayerId, null, new Dictionary<string, PlayerDataObject> {
            { KEY_PLAYER_NAME, new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) },
        });

    public async void CreateLobby(string lobbyName, int maxPlayers, bool isPrivate) {
        Player player = GetPlayer();

        CreateLobbyOptions options = new()
        {
            Player = player,
            IsPrivate = isPrivate,
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        joinedLobby = lobby;
        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        Debug.Log("Created Lobby " + lobby.Name);
    }

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

    public async void JoinLobbyByCode(string lobbyCode) {
        Player player = GetPlayer();

        Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode, new JoinLobbyByCodeOptions {
            Player = player
        });

        joinedLobby = lobby;

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async void JoinLobby(Lobby lobby) {
        Player player = GetPlayer();

        joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobby.Id, new JoinLobbyByIdOptions {
            Player = player
        });

        OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
    }

    public async void UpdatePlayerName(string pName) {
        playerName = pName;

        if (joinedLobby == null) 
            return;
        
        try {
            UpdatePlayerOptions options = new();

            options.Data = new Dictionary<string, PlayerDataObject> {
                {
                    KEY_PLAYER_NAME, new PlayerDataObject(
                        visibility: PlayerDataObject.VisibilityOptions.Public,
                        value: playerName)
                }
            };

            string playerId = AuthenticationService.Instance.PlayerId;

            Lobby lobby = await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, playerId, options);
            joinedLobby = lobby;

            OnJoinedLobbyUpdate?.Invoke(this, new LobbyEventArgs { lobby = joinedLobby });
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void QuickJoinLobby() {
        try {
            QuickJoinLobbyOptions options = new();

            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);
            joinedLobby = lobby;

            OnJoinedLobby?.Invoke(this, new LobbyEventArgs { lobby = lobby });
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void LeaveLobby()
    {
        if (joinedLobby == null) 
            return;
        
        try {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            joinedLobby = null;

            OnLeftLobby?.Invoke(this, EventArgs.Empty);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (!IsLobbyHost()) 
            return;
        
        try {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
        } catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }
}