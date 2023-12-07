using System;
using System.Linq;
using FTS.Tools.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FTS.Managers
{
    public enum GameType : byte {None, Singleplayer, Multiplayer}
    
    [DisallowMultipleComponent]
    internal sealed class MenuPlayManager : BaseManager
    {
        [Dropdown(nameof(GetSceneNames))]
        [SerializeField] private string _selectedScene;
        
        [SerializeField] private GameSettings _gameSettings = new();

        public void SetGameType(GameType type) => _gameSettings.SetGameType(type);
        
        private string[] GetSceneNames() =>
            EditorBuildSettings.scenes.Where(scene => scene.path != SceneManager.GetActiveScene().path)
                                      .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                                      .ToArray();

        public void StartGame() => SceneManager.LoadScene(_selectedScene);
    }

    [Serializable]
    internal class GameSettings
    {
        [field: SerializeField, ReadOnly] public GameType GameType { get; private set; } = GameType.None;
        [field: SerializeField] public LobbySettings LobbySettings { get; private set; }
        [field: SerializeField] public WorldSettings WorldSettings { get; private set; }

        public void SetGameType(GameType gameType) => GameType = gameType;
        public void SetLobbySettings(LobbySettings lobbySettings) => LobbySettings = lobbySettings;
        public void SetWorldSettings(WorldSettings worldSettings) => WorldSettings = worldSettings;
    }

    [Serializable]
    internal struct LobbySettings
    {
        [ReadOnly] public string r_lobbyName;
        [ReadOnly] public int r_playerNumber;

        public LobbySettings(string lobbyName, int playerNumber)
        {
            r_lobbyName = lobbyName;
            r_playerNumber = playerNumber;
        }
    }
    
    [Serializable]
    internal struct WorldSettings
    {
        [ReadOnly] public string r_worldName;

        public WorldSettings(string worldName)
        {
            r_worldName = worldName;
        }
    }
}