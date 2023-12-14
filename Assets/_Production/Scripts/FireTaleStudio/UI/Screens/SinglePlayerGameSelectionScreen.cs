using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerGameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => StartGame(lobbyManager));
        }

        private void StartGame(LobbyManager lobbyManager)
        {
            GameMap gameMap = ItemDatabase.GetAllOfType<GameMap>()[0];
            lobbyManager.SetMapSettings(new MapSettings(gameMap.Id, new MapData()));
            SceneManager.LoadScene(gameMap.Name);
        }
    }
}