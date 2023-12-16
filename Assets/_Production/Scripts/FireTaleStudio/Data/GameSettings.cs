using System;
using FTS.Tools.Attributes;
using UnityEngine;

namespace FTS.Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "FTS/Game Settings")]
    internal sealed class GameSettings : ScriptableObject
    {
        [field: SerializeField, ReadOnly] public GameType GameType { get; private set; } = GameType.None;
        [field: SerializeField] public PlayerSettings PlayerSettings { get; private set; }
        [field: SerializeField] public LobbySettings LobbySettings { get; private set; }
        [field: SerializeField] public MapSettings MapSettings { get; private set; }

        public void SetGameType(GameType gameType) => GameType = gameType;
        public void SetLobbySettings(LobbySettings lobbySettings) => LobbySettings = lobbySettings;
        public void SetMapSettings(MapSettings mapSettings) => MapSettings = mapSettings;
        public void SetMapDataSettings(MapData mapData) => MapSettings = new MapSettings(MapSettings.r_mapId, mapData);
        public void SetPlayerSettings(PlayerSettings playerSettings) => PlayerSettings = playerSettings;
        public void SetMapId(int mapId) => MapSettings = new MapSettings(mapId, MapSettings.r_mapData);
    }
    
    internal enum GameType : byte {None, Singleplayer, Multiplayer}
    
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
        [ReadOnly] public string r_lobbyPassword;

        public LobbySettings(string lobbyName, int playerNumber, string lobbyPassword)
        {
            r_lobbyName = lobbyName;
            r_playerNumber = playerNumber;
            r_lobbyPassword = lobbyPassword;
        }
    }
    
    [Serializable]
    internal struct MapSettings
    {
        [ReadOnly] public int r_mapId;
        [ReadOnly] public MapData r_mapData;

        public MapSettings(int mapId, MapData mapData)
        {
            r_mapId = mapId;
            r_mapData = mapData;
        }
    }
    
    [Serializable]
    internal struct MapData
    {
        [ReadOnly] public float r_aiDifficulty;
        [ReadOnly] public float r_resourcesDrop;
        [ReadOnly] public float r_craftCost;
        [ReadOnly] public float r_upgradeCost;
        [ReadOnly] public float r_waveCooldown;
        [ReadOnly] public bool r_monsterRarity;

        public MapData(float aiDifficulty, float resourcesDrop, float craftCost, float upgradeCost, float waveCooldown, bool monsterRarity)
        {
            r_aiDifficulty = aiDifficulty;
            r_resourcesDrop = resourcesDrop;
            r_craftCost = craftCost;
            r_upgradeCost = upgradeCost;
            r_waveCooldown = waveCooldown;
            r_monsterRarity = monsterRarity;
        }
    }
}