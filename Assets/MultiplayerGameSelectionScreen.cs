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

        protected override void OnInitialize(IManager manager)
        {
            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gameTypeDropdown.onValueChanged.AddListener(value => _lobbyType = (LobbyType)value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _playGame.onClick.AddListener(() => StartGame(menuPlayManager));
        }

        private void StartGame(MenuPlayManager menuPlayManager)
        {
            menuPlayManager.SetMapSettings(new MapSettings(_mapName));
            menuPlayManager.SetLobbySettings(new LobbySettings(
                _gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber,
                _lobbyType));
        }
    }
}
