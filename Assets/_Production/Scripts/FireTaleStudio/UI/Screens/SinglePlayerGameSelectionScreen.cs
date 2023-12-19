using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using FTS.UI.GameLobby;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerGameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private MapSettingUi _mapSettingUi;
        [SerializeField] private MapDisplayUi _mapDisplayUi;
        
        [Header("Host Components"), Space(2)] 
        [SerializeField] private MapSelectionUi _mapSelectionUi;
        
        [Header("Components"), Space(2)]
        [SerializeField] private Button _playButton;

        private MapData _mapData = new(1, 1, 1, 1, 20, true);
        private int _mapId;
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            _playButton.onClick.AddListener(() => StartGame(lobbyManager));

            _mapId = ItemDatabase.GetAllOfType<GameMap>()[0].Id;
            _mapDisplayUi.UpdateMap(_mapId);
            
            // Host Data
            _mapSettingUi.Initialize(mapData => _mapData = mapData);
            _mapSelectionUi.Initialize(id => _mapId = id);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _playButton.onClick.RemoveAllListeners();
        }

        private void StartGame(LobbyManager lobbyManager)
        {
            GameMap gameMap = ItemDatabase.Get<GameMap>(_mapId);
            lobbyManager.SetMapSettings(new MapSettings(_mapId, _mapData));
            SceneManager.LoadScene(gameMap.Name);
        }
    }
}