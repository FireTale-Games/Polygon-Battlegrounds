using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FTS.Tools.Utilities;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FTS.Multiplayer
{
    public class Multiplayer : MonoBehaviour
    {
        [SerializeField] private string _lobbyName = "Lobby";
        [SerializeField] private int _maxPlayers = 4;
        
        public static Multiplayer Instance { get; private set; }

        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        
        private Lobby _currentLobby;
        
        private const float k_lobbyHeartbeatInterval = 20.0f;
        private const float k_lobbyPoolInterval = 65.0f;
        private const string k_keyJoinCode = "RelayJoinCode";
        private const string k_dtlsEncryption = "dtls";

        private readonly CountdownTimer _heartbeatTimer = new(k_lobbyHeartbeatInterval);
        private readonly CountdownTimer _pollForUpdatesTimer = new(k_lobbyPoolInterval);
        
        private async void Start()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            await Authenticate();

            _heartbeatTimer.OnTimeStop += () =>
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                HandleHeartbeatAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _heartbeatTimer.Start();
            };
            
            _pollForUpdatesTimer.OnTimeStop += () =>
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                HandlePoolForUpdatesAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _pollForUpdatesTimer.Start();
            };
        }

        private async Task Authenticate() => 
            await Authenticate("Player" + Random.Range(0, 100000));

        private async Task Authenticate(string playerName)
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            { 
                InitializationOptions options = new();
                options.SetProfile(playerName);
                await UnityServices.InitializeAsync(options);
            }

            AuthenticationService.Instance.SignedIn += () => Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerId = AuthenticationService.Instance.PlayerId;
                PlayerName = AuthenticationService.Instance.PlayerName;
            }
        }
        
        public async Task CreateLobby()
        {
            try
            {
                Allocation allocation = await AllocateRelay();
                string relayJoinCode = await GetRelayJoinCode(allocation);

                CreateLobbyOptions options = new()
                {
                    IsPrivate = false
                };

                _currentLobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, _maxPlayers, options);
                Debug.Log("Created Lobby: " + _currentLobby.Name + " with code " + _currentLobby.LobbyCode);
                
                _heartbeatTimer.Start();
                _pollForUpdatesTimer.Start();

                await LobbyService.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                    {
                        { k_keyJoinCode, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode) } 
                    }
                });
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(
                    allocation, k_dtlsEncryption));

                NetworkManager.Singleton.StartHost();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Failed to create lobby: " + e.Message);
            }
        }

        public async Task QuickJoinLobby()
        {
            try
            {
                _currentLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                _pollForUpdatesTimer.Start();

                string relayJoinCode = _currentLobby.Data[k_keyJoinCode].Value;
                JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);
                
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, k_dtlsEncryption));
                NetworkManager.Singleton.StartClient();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Failed to join lobby: " + e.Message);
            }
        }
        
        private async Task<Allocation> AllocateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(_maxPlayers - 1);
                return allocation;
            }
            catch (RelayServiceException e)
            {
                Debug.Log("Failed to allocate relay: " + e.Message);
                return default;
            }
        }

        private async Task<string> GetRelayJoinCode(Allocation allocation)
        {
            try
            {
                return await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            }
            catch (RelayServiceException e)
            {
                Debug.Log("Failed to get relay join code: " + e.Message);
                return default;
            }
        }

        private async Task<JoinAllocation> JoinRelay(string relayJoinCode)
        {
            try
            {
                return await RelayService.Instance.JoinAllocationAsync(relayJoinCode);
            }
            catch (RelayServiceException e)
            {
                Debug.Log("Failed to join relay: " + e.Message);
                return default;
            }
        }
        
        private async Task HandleHeartbeatAsync()
        {
            try
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
                Debug.Log("Sent heartbeat ping to lobby: " + _currentLobby.Name);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Failed to heartbeat lobby: " + e.Message);
            }
        }

        private async Task HandlePoolForUpdatesAsync()
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_currentLobby.Id);
                Debug.Log("Polled for updates on lobby: " + lobby.Name);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log("Failed to pool for updates on lobby: " + e.Message);
            }
        }
    }
}