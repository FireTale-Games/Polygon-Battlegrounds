using System;
using System.Collections.Generic;
using FTS.Managers;
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
        [SerializeField] private Sprite[] _lockUnlockSprites;
        [SerializeField] private LobbyGameUi _lobbyGameUi;
        [SerializeField] private RectTransform _lobbyList;
        [SerializeField] private MenuButtonUi _createLobby;
        [SerializeField] private Button _refreshLobby;

        [Header("Description Data"), Space(2)]
        [SerializeField] private LobbyPlayerDescriptionUi _lobbyPlayerDescriptionUi;
        [SerializeField] private RectTransform _hostGroup;
        [SerializeField] private RectTransform _playersGroup;

        private Action<Lobby> OnLobbyJoin;
        private Action<Lobby> OnLobbySelect;
        private Action OnLobbyShow;

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            // Refresh
            lobbyManager.OnLobbyListChanged += UpdateLobbyList_EventHandler;
            _refreshLobby.onClick.AddListener(lobbyManager.RefreshLobbyList);
            
            // Etc.
            OnLobbyJoin = lobbyManager.JoinLobby;
            OnLobbySelect = DisplayLobbyDescription;
            OnLobbyShow = LobbyShow;
            return;

            async void LobbyShow() => await lobbyManager.Authenticate();
        }

        #region LOBBY_LOBBY_DISPLAY

        private void DisplayLobbyDescription(Lobby lobby)
        {
            RemoveLobbyPlayerDescriptions();
            
            List<Player> players = lobby.Players;
            Instantiate(_lobbyPlayerDescriptionUi, _hostGroup).Initialize(players[0].Data["PlayerName"].Value, "Host");
            for (int i = 1; i < players.Count; i++)
                Instantiate(_lobbyPlayerDescriptionUi, _playersGroup).Initialize(players[i].Data["PlayerName"].Value, "Member");
        }
        
        private void RemoveLobbyPlayerDescriptions()
        {
            ILobbyPlayerDescriptionUi[] lobbyPlayerDescriptionUi = GetComponentsInChildren<ILobbyPlayerDescriptionUi>();
            for (int i = lobbyPlayerDescriptionUi.Length - 1; i >= 0; i--)
                lobbyPlayerDescriptionUi[i].Destroy();
        }
        
        #endregion
        
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
                    OnLobbySelect);
                    
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
            OnLobbySelect = null;
            OnLobbyShow = null;
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnLobbyShow?.Invoke();
            RemoveLobbyPlayerDescriptions();
        }
    }
}