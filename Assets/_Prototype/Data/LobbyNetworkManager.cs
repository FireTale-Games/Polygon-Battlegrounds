using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FTS.Data;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace FTS.Managers
{
    internal sealed class LobbyNetworkManager : NetworkBehaviour, ILobbyNetworkUpdate
    {
        public EventHandler<MapSettings> OnSettingsUpdate { get; set; }
        
        public void StartHost(string lobbyId)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", GetPortFromHash(lobbyId));
            NetworkManager.Singleton.StartHost();
        }

        public void StartClient(string lobbyId)
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("127.0.0.1", GetPortFromHash(lobbyId));
            NetworkManager.Singleton.StartClient();
        }
        
        public void UpdateLobby(MapSettings settings)
        {
            if (!IsServer || !IsHost)
                return;
            
            byte[] binaryData = SerializeToBinary(settings);
            LobbyUpdateClientRpc(binaryData);
        }
        
        private byte[] SerializeToBinary(MapSettings settings)
        {
            using MemoryStream memoryStream = new();
            BinaryFormatter formatter = new();
            formatter.Serialize(memoryStream, settings);
            return memoryStream.ToArray();
        }

        private MapSettings DeserializeFromBinary(byte[] binaryData)
        {
            using MemoryStream memoryStream = new(binaryData);
            BinaryFormatter formatter = new();
            return (MapSettings)formatter.Deserialize(memoryStream);
        }
        
        [ClientRpc]
        private void LobbyUpdateClientRpc(byte[] binaryData)
        {
            MapSettings settings = DeserializeFromBinary(binaryData);
            OnSettingsUpdate?.Invoke(this, settings);
        }
        
        private ushort GetPortFromHash(string lobbyHostId) => (ushort)(1024 + Mathf.Abs(Animator.StringToHash(lobbyHostId)) % (65535 - 1024));
    }

    internal interface ILobbyNetworkUpdate
    {
        public EventHandler<MapSettings> OnSettingsUpdate { get; set; }
    }
}