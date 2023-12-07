using System;
using System.Linq;
using FTS.Tools.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.Managers
{
    public enum GameType : byte {None, Singleplayer, Multiplayer}
    public enum LobbyType : byte {Public, Private}
    
    [DisallowMultipleComponent]
    internal sealed class MenuPlayManager : BaseManager
    {
        [Dropdown(nameof(GetSceneNames))]
        [SerializeField] private string _selectedScene;
        
        [field: SerializeField] public GameSettings GameSettings { get; private set; } = new();

        public void SetGameType(GameType type) => GameSettings.SetGameType(type);
        public void SetLobbySettings(LobbySettings lobbySettings) => GameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => GameSettings.SetMapSettings(mapSettings);
        
        private string[] GetSceneNames() =>
            EditorBuildSettings.scenes.Where(scene => scene.path != SceneManager.GetActiveScene().path)
                                      .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                                      .ToArray();

        public void StartGame() => SceneManager.LoadScene(_selectedScene);
    }

    [Serializable]
    internal sealed class GameSettings
    {
        [field: SerializeField, ReadOnly] public GameType GameType { get; private set; } = GameType.None;
        [field: SerializeField] public LobbySettings LobbySettings { get; private set; }
        [field: SerializeField] public MapSettings MapSettings { get; private set; }

        internal void SetGameType(GameType gameType) => GameType = gameType;
        internal void SetLobbySettings(LobbySettings lobbySettings) => LobbySettings = lobbySettings;
        internal void SetMapSettings(MapSettings mapSettings) => MapSettings = mapSettings;
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