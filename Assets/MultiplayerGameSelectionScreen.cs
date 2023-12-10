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
        [SerializeField] private TMP_Dropdown _gameTypeDropdown;
        [SerializeField] private TMP_InputField _gameNameInputField;

        private string _mapName = "WorldOne_Scene";
        private int _playerNumber = 2;
        private LobbyType _lobbyType = LobbyType.Public;
        private string _gameName = "";
        
        protected override void BindToLobbyManager(Managers.LobbyManager lobbyManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gameTypeDropdown.onValueChanged.AddListener(value => _lobbyType = (LobbyType)value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _playGame.onClick.AddListener(() => StartGame(lobbyManager));
            _playGame.onClick.AddListener(OnCreateLobby);

            return;
            async void OnCreateLobby() => await lobbyManager.CreateLobby();
        }

        private void StartGame(Managers.LobbyManager lobbyManager)
        {
            lobbyManager.SetMapSettings(new MapSettings(_mapName));
            lobbyManager.SetLobbySettings(new LobbySettings(
                _gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber,
                _lobbyType));
        }
    }
}
