using System;
using System.Collections.Generic;
using FTS.Managers;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
// ReSharper disable ConvertClosureToMethodGroup

namespace FTS.UI.Screens
{
    internal sealed class LobbyScreen : MenuScreenBase
    {
        [SerializeField] private RectTransform _lobbyList;
        [SerializeField] private Transform _lobbySingleTemplate;
        [SerializeField] private Button _joinGame;
        [SerializeField] private Button _refreshLobby;
        [SerializeField] private Button _backButton;

        private Action<bool> OnLobbyShow;
        private Action<Lobby> OnLobbyJoin;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is MultiplayerManager multiplayerManager)
                BindToMultiplayerManager(multiplayerManager);

            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayerManager(menuPlayManager);
        }

        private void BindToMultiplayerManager(MultiplayerManager multiplayerManager)
        {
            OnLobbyShow += value => multiplayerManager.SetNetworkConnection(value);
            _backButton.onClick.AddListener(() => OnLobbyShow?.Invoke(false));
        }
        
        private void BindToMenuPlayerManager(MenuPlayManager menuPlayManager)
        {
            OnLobbyShow = OnShowAuthenticate;
            OnLobbyJoin = menuPlayManager.JoinLobby;
            menuPlayManager.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
            
            _refreshLobby.onClick.AddListener(() => menuPlayManager.RefreshLobbyList());
            
            return;
            async void OnShowAuthenticate(bool value) => await menuPlayManager.Authenticate();
        }

        private void LobbyManager_OnLobbyListChanged(object sender, MenuPlayManager.OnLobbyListChangedEventArgs e) =>
            UpdateLobbyList(e.lobbyList);
        
        private void UpdateLobbyList(List<Lobby> lobbyList) {
            foreach (Transform child in _lobbyList) {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbyList) {
                Transform lobbySingleTransform = Instantiate(_lobbySingleTemplate, _lobbyList);
                lobbySingleTransform.gameObject.SetActive(true);
                LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUI>();
                lobbyListSingleUI.UpdateLobby(OnLobbyJoin, lobby);
            }
        }
        
        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnLobbyShow?.Invoke(true);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _refreshLobby.onClick.RemoveAllListeners();
            _joinGame.onClick.RemoveAllListeners();
        }
    }
}