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

        protected override void BindToLobbyManager(Managers.LobbyManager lobbyManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => StartGame(lobbyManager));
        }

        private void StartGame(Managers.LobbyManager lobbyManager)
        {
            lobbyManager.SetMapSettings(new MapSettings(_mapName));
            SceneManager.LoadScene(lobbyManager.GameSettings.MapSettings.r_mapName);
        }
    }
}