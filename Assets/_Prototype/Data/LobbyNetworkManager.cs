using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FTS.Data;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace FTS.Managers
{
    [Serializable]
    internal readonly struct PlayerRef
    {
        public readonly string r_playerName;
        public readonly string r_playerId;

        public PlayerRef(string playerName, string playerId)
        {
            r_playerName = playerName;
            r_playerId = playerId;
        }
    }
    
    [Serializable]
    internal readonly struct LobbyRef
    {
        public readonly PlayerRef[] r_playerRefs;
        public readonly bool r_isHost;
        public readonly string r_lobbyCode;

        public LobbyRef(PlayerRef[] playerRefs, bool isHost, string lobbyCode)
        {
            r_playerRefs = playerRefs;
            r_isHost = isHost;
            r_lobbyCode = lobbyCode;
        }
    }
    
    internal sealed class LobbyNetworkManager : NetworkBehaviour, ILobbyNetworkUpdate
    {
        public EventHandler<MapSettings> OnSettingsUpdate { get; set; }
        public EventHandler<LobbyRef> OnLobbyPlayersUpdate { get; set; }
        public EventHandler<bool> OnPlayerLeave { get; set; }
        private const string k_ipAddress = "127.0.0.1";

        public void StartHost(Lobby lobby)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(k_ipAddress, GetPortFromHash(lobby.Id));
            NetworkManager.Singleton.StartHost();
            LobbyJoin(lobby);
        }

        public void StartClient(Lobby lobby)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(k_ipAddress, GetPortFromHash(lobby.Id));
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.OnClientConnectedCallback += OnServerStarted;
            return;

            void OnServerStarted(ulong clientId)
            {
                if (clientId != NetworkManager.Singleton.LocalClientId) 
                    return;
                
                LobbyJoin(lobby);
                NetworkManager.Singleton.OnClientConnectedCallback -= OnServerStarted;
            }
        }
        
        public void UpdateLobby(MapSettings settings)
        {
            if (!IsServer || !IsHost)
                return;
            
            byte[] binaryData = SerializeToBinary(settings);
            LobbyUpdateClientRpc(binaryData);
        }

        public void LobbyJoin(Lobby lobby)
        {
            PlayerRef[] playerRefs = new PlayerRef[lobby.Players.Count];
            for (int i = 0; i < playerRefs.Length; i++)
            {
                Player player = lobby.Players[i];
                playerRefs[i] = new PlayerRef(player.Data["PlayerName"].Value, player.Id);
            }

            LobbyRef lobbyRef = new(playerRefs, false, lobby.LobbyCode);
            byte[] binaryData = SerializeToBinary(lobbyRef);
            LobbyJoinServerRpc(binaryData);
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void LobbyJoinServerRpc(byte[] lobbyBinaryData) => 
            LobbyJoinClientRpc(lobbyBinaryData);

        [ClientRpc]
        private void LobbyJoinClientRpc(byte[] lobbyBinaryData)
        {
            LobbyRef lobbyRef = DeserializeFromBinary<LobbyRef>(lobbyBinaryData);
            if (IsServer || IsHost)
                lobbyRef = new LobbyRef(lobbyRef.r_playerRefs, true, lobbyRef.r_lobbyCode);
            
            OnLobbyPlayersUpdate?.Invoke(this, lobbyRef);
        }

        [ClientRpc]
        private void LobbyUpdateClientRpc(byte[] binaryData)
        {
            MapSettings settings = DeserializeFromBinary<MapSettings>(binaryData);
            OnSettingsUpdate?.Invoke(this, settings);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void RemovePlayerServerRpc(string removePlayerId, bool isVoluntary)
        {
            if (IsServer || IsHost)
                RemovePlayerClientRpc(removePlayerId, isVoluntary);
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(string playerId, bool isVoluntary)
        {
            if (AuthenticationService.Instance.PlayerId != playerId) 
                return;
            
            NetworkManager.Singleton.Shutdown();
            OnPlayerLeave?.Invoke(this, isVoluntary);
        }
        
        public void RemoveAllPlayers()
        {
            if (IsServer || IsHost)
                RemoveAllPlayersClientRpc();
        }
        
        [ClientRpc]
        private void RemoveAllPlayersClientRpc()
        {
            NetworkManager.Singleton.Shutdown();
            OnPlayerLeave?.Invoke(this, IsServer || IsHost);
        }

        private ushort GetPortFromHash(string lobbyHostId) => (ushort)(1024 + Mathf.Abs(Animator.StringToHash(lobbyHostId)) % (65535 - 1024));
        
        private byte[] SerializeToBinary<T>(T settings)
        {
            using MemoryStream memoryStream = new();
            BinaryFormatter formatter = new();
            formatter.Serialize(memoryStream, settings);
            return memoryStream.ToArray();
        }

        private T DeserializeFromBinary<T>(byte[] binaryData)
        {
            using MemoryStream memoryStream = new(binaryData);
            BinaryFormatter formatter = new();
            return (T)formatter.Deserialize(memoryStream);
        }
    }

    internal interface ILobbyNetworkUpdate
    {
        public EventHandler<MapSettings> OnSettingsUpdate { get; set; }
        public EventHandler<LobbyRef> OnLobbyPlayersUpdate { get; set; }
        public EventHandler<bool> OnPlayerLeave { get; set; }
    }
}