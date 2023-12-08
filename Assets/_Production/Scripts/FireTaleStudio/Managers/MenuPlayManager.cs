using System.Linq;
using FTS.Data;
using FTS.Tools.Attributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using PlayerSettings = FTS.Data.PlayerSettings;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    internal sealed class MenuPlayManager : BaseManager
    {
        [Dropdown(nameof(GetSceneNames))] [SerializeField]
        private string _selectedScene;

        [field: SerializeField] public GameSettings GameSettings { get; private set; }
        public void SetGameType(GameType type) => GameSettings.SetGameType(type);
        public void SetPlayerSettings(PlayerSettings playerSettings) => GameSettings.SetPlayerSettings(playerSettings);
        public void SetLobbySettings(LobbySettings lobbySettings) => GameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => GameSettings.SetMapSettings(mapSettings);

        private string[] GetSceneNames() =>
            EditorBuildSettings.scenes.Where(scene => scene.path != SceneManager.GetActiveScene().path)
                .Select(scene => System.IO.Path.GetFileNameWithoutExtension(scene.path))
                .ToArray();

        public void StartGame() => SceneManager.LoadScene(_selectedScene);
    }
}