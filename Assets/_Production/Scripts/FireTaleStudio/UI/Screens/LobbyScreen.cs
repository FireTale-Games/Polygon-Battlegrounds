using System;
using System.Collections.Generic;
using FTS.Managers;
using FTS.UI.GameLobby;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class LobbyScreen : MenuScreenBase
    {
        [Header("Search Data")]
        [SerializeField] private TMP_InputField _searchInputField;
        [SerializeField] private Button _searchInputButton;

        [Header("Lobby Data"), Space(2)] 
        [SerializeField] private MenuScreenBase _gameScreen;
        [SerializeField] private Sprite[] _lockUnlockSprites;
        [SerializeField] private LobbyGameUi _lobbyGameUi;
        [SerializeField] private RectTransform _lobbyList;
        [SerializeField] private Button _refreshLobby;

        [Header("Create Lobby"), Space(2)] 
        [SerializeField] private CreateGameScreen _createGameScreen;
        [SerializeField] private Button _createLobby;

        [Header("Description Data"), Space(2)] 
        [SerializeField] private JoinGameScreen _joinGameScreen;

        private Action<Lobby> OnLobbyJoin;
        private Action OnLobbyShow;

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            // Refresh
            lobbyManager.OnLobbyListChanged += UpdateLobbyList_EventHandler;
            _refreshLobby.onClick.AddListener(() => { lobbyManager.RefreshLobbyList(); _joinGameScreen.SetVisibility(false); });
            
            // Create New Lobby
            _createLobby.onClick.AddListener(_createGameScreen.SetVisibility);
            
            // Join lobby
            OnLobbyJoin = lobbyManager.JoinLobby;
            _joinGameScreen.Initialize(lobbyManager.JoinLobby);
            
            // Etc.
            OnLobbyShow = LobbyShow;
            return;

            async void LobbyShow() => await lobbyManager.Authenticate();
        }
        
        #region LOBBY_LIST_UPDATE

        private void UpdateLobbyList_EventHandler(object sender, Lobby[] lobbyList) => 
            UpdateLobbyList(lobbyList);

        private void UpdateLobbyList(IEnumerable<Lobby> lobbyList) {
            foreach (Transform child in _lobbyList) 
                Destroy(child.gameObject);

            foreach (Lobby lobby in lobbyList) {
                LobbyGameUiData lobbyGameUiData = new(
                    lobby.Data["Password"].Value != string.Empty ? _lockUnlockSprites[0] : _lockUnlockSprites[1],
                    OnLobbyJoin,
                    _joinGameScreen.DisplayLobbyDescription,
                    _gameScreen);
                    
                LobbyGameUi lobbyGameUi = Instantiate(_lobbyGameUi, _lobbyList);
                lobbyGameUi.Initialize(lobbyGameUiData, lobby);
            }
        }

        #endregion
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _refreshLobby.onClick.RemoveAllListeners();
            OnLobbyJoin = null;
            OnLobbyShow = null;
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnLobbyShow?.Invoke();
            _joinGameScreen.SetVisibility(false);
        }
    }
}