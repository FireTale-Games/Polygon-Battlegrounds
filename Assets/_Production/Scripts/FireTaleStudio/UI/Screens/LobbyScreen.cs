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
        [SerializeField] private Sprite[] _lockUnlock;
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
        
        private void UpdateLobbyList(List<Lobby> lobbyList) {
            foreach (Transform child in _lobbyList) 
                Destroy(child.gameObject);

            foreach (Lobby lobby in lobbyList) {
                LobbyGameUiData lobbyGameUiData = new(
                    lobby.IsPrivate ? _lockUnlock[0] : _lockUnlock[1],
                    lobby.Name,
                    lobby.Players.Count,
                    lobby.MaxPlayers,
                    OnLobbyJoin,
                    OnLobbySelect);
                    
                LobbyGameUi lobbyGameUi = Instantiate(_lobbyGameUi, _lobbyList);
                lobbyGameUi.Initialize(lobbyGameUiData, lobby);
            }
        }

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            lobbyManager.OnLobbyListChanged += UpdateLobbyList_Event;
            _refreshLobby.onClick.AddListener(lobbyManager.RefreshLobbyList);
            OnLobbyJoin = lobbyManager.JoinLobby;
        }

        private void UpdateLobbyList_Event(object sender, LobbyManager.OnLobbyListChangedEventArgs e) => 
            UpdateLobbyList(e.lobbyList);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _refreshLobby.onClick.RemoveAllListeners();
            OnLobbyJoin = null;
            OnLobbySelect = null;
        }
    }
}