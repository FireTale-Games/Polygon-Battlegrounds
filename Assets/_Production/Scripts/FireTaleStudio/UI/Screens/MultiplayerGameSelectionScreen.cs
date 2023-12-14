using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using FTS.Tools.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class MultiplayerGameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;
        [SerializeField] private TMP_Dropdown _playerDropdown;
        [SerializeField] private TMP_InputField _gamePasswordField;
        [SerializeField] private TMP_InputField _gameNameInputField;
        
        private int _playerNumber = 2;
        private string _lobbyPassword = string.Empty;
        private string _gameName = "";
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gamePasswordField.onValueChanged.AddListener(value => _lobbyPassword = value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _playGame.onClick.AddListener(() => SetLobbyData(lobbyManager));
            _playGame.onClick.AddListener(OnCreateLobby);

            return;
            async void OnCreateLobby() => await lobbyManager.CreateLobby();
        }

        private void SetLobbyData(LobbyManager lobbyManager)
        {
            GameMap gameMap = ItemDatabase.GetAllOfType<GameMap>()[0];
            MapData mapData = new(0.5f, 1.0f, 1.0f, 1.0f, 20.0f, true);
            lobbyManager.SetMapSettings(new MapSettings(gameMap.Id, mapData));
            lobbyManager.SetLobbySettings(new LobbySettings(
                _gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber, 
                _lobbyPassword));
        }
    }
}
