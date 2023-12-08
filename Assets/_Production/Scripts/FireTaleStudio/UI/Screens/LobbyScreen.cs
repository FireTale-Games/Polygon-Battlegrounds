using System;
using FTS.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class LobbyScreen : MenuScreenBase
    {
        [SerializeField] private Button _joinGame;
        [SerializeField] private Button _refreshLobby;
        [SerializeField] private Button _backButton;

        private Action<bool> OnLobbyShow;
        
        protected override void Awake()
        {
            base.Awake();
            InitializeButtons();
        }
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is not MultiplayerManager multiplayerManager)
                return;
            
            OnLobbyShow = value=> multiplayerManager.SetNetworkConnection(value);
            _backButton.onClick.AddListener(() => OnLobbyShow?.Invoke(false));
        }
        
        private void InitializeButtons()
        {
            _refreshLobby.onClick.AddListener(RefreshLobbyList);
            _joinGame.onClick.AddListener(JoinLobby);
        }

        private void RefreshLobbyList()
        {
            
        }
        
        private void JoinLobby()
        {
            
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