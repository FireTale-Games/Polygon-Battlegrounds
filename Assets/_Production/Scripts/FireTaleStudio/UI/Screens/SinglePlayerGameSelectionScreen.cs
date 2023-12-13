using FTS.Data;
using FTS.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerGameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;
        private string _mapName = "WorldOne_Scene";

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => StartGame(lobbyManager));
        }

        private void StartGame(LobbyManager lobbyManager)
        {
            lobbyManager.SetMapSettings(new MapSettings(_mapName, new MapData()));
            SceneManager.LoadScene(lobbyManager.GameSettings.MapSettings.r_mapName);
        }
    }
}