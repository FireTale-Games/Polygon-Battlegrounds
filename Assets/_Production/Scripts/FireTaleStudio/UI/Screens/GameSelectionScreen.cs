using System;
using FTS.Data;
using FTS.Managers;
using FTS.Tools.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;
        [SerializeField] private GameObject _multiplayerOptions;
        [SerializeField] private TMP_Dropdown _playerDropdown;
        [SerializeField] private TMP_Dropdown _gameTypeDropdown;
        [SerializeField] private TMP_InputField _gameNameInputField;

        private string _mapName = "WorldOne_Scene";
        private int _playerNumber = 2;
        private LobbyType _lobbyType = LobbyType.Public;
        private string _gameName = "";
        private Func<GameType> OnShowScreen; 

        protected override void OnInitialize(IManager manager)
        {
            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            OnShowScreen += ShowScreen;
            
            foreach (IMapButtonUi mapButton in GetComponentsInChildren<IMapButtonUi>())
                mapButton.MapButton.onClick.AddListener(() => _mapName = mapButton.MapName);
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gameTypeDropdown.onValueChanged.AddListener(value => _lobbyType = (LobbyType)value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => StartGame(menuPlayManager));
            
            return;
            GameType ShowScreen()
            {
                menuPlayManager.SetMapSettings(new MapSettings());
                menuPlayManager.SetLobbySettings(new LobbySettings());
                return menuPlayManager.GameSettings.GameType;
            }
        }

        private void StartGame(MenuPlayManager menuPlayManager)
        {
            menuPlayManager.SetMapSettings(new MapSettings(_mapName));
            if (menuPlayManager.GameSettings.GameType == GameType.Singleplayer)
            {
                SceneManager.LoadScene(menuPlayManager.GameSettings.MapSettings.r_mapName);
                return;
            }
            
            menuPlayManager.SetLobbySettings(new LobbySettings(_gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber, _lobbyType));
            SceneManager.LoadScene("Lobby_Scene");
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            _multiplayerOptions.SetActive(OnShowScreen?.Invoke() == GameType.Multiplayer);
        }
    }
}