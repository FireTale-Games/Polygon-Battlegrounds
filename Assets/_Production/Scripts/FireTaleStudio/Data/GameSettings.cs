using System;
using FTS.Tools.Attributes;
using UnityEngine;

namespace FTS.Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "FTS/Data/Game Settings")]
    internal sealed class GameSettings : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public GameType GameType { get; private set; } = GameType.None;
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        [field: SerializeField] public LobbySettings LobbySettings { get; private set; }
        [field: SerializeField] public MapSettings MapSettings { get; private set; }

        internal void SetGameType(GameType gameType) => GameType = gameType;
        internal void SetLobbySettings(LobbySettings lobbySettings) => LobbySettings = lobbySettings;
        internal void SetMapSettings(MapSettings mapSettings) => MapSettings = mapSettings;
        internal void SetPlayerSettings(PlayerSettings playerSettings) => PlayerSettings = playerSettings;
    }
    
    internal enum GameType : byte {None, Singleplayer, Multiplayer}
    internal enum LobbyType : byte {Public, Private}
    
    [Serializable]
    internal struct PlayerSettings
    {
        [ReadOnly] public int r_playerName;

        public PlayerSettings(int playerName)
        {
            r_playerName = playerName;
        }
    }
    
    [Serializable]
    internal struct LobbySettings
    {
        [ReadOnly] public string r_lobbyName;
        [ReadOnly] public int r_playerNumber;
        [ReadOnly] public LobbyType r_lobbyType;

        public LobbySettings(string lobbyName, int playerNumber, LobbyType lobbyType)
        {
            r_lobbyName = lobbyName;
            r_playerNumber = playerNumber;
            r_lobbyType = lobbyType;
        }
    }
    
    [Serializable]
    internal struct MapSettings
    {
        [ReadOnly] public string r_mapName;

        public MapSettings(string mapName)
        {
            r_mapName = mapName;
        }
    }
}