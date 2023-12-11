using FTS.Data;
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

        private string _mapName = "WorldOne_Scene";
        private int _playerNumber = 2;
        private string _lobbyPassword = string.Empty;
        private string _gameName = "";
        
        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gamePasswordField.onValueChanged.AddListener(value => _lobbyPassword = value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _playGame.onClick.AddListener(() => StartGame(lobbyManager));
            _playGame.onClick.AddListener(OnCreateLobby);

            return;
            async void OnCreateLobby() => await lobbyManager.CreateLobby();
        }

        private void StartGame(LobbyManager lobbyManager)
        {
            lobbyManager.SetMapSettings(new MapSettings(_mapName));
            lobbyManager.SetLobbySettings(new LobbySettings(
                _gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber, 
                _lobbyPassword));
        }
    }
}
