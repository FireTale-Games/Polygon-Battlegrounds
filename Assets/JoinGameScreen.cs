using System;
using System.Collections.Generic;
using DG.Tweening;
using FTS.Managers;
using FTS.Tools.Extensions;
using FTS.UI.GameLobby;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class JoinGameScreen : MenuScreenBase
    {
        [SerializeField] private Button _joinButton;
        [SerializeField] private LobbyPlayerDescriptionUi _lobbyPlayerDescriptionUi;
        [SerializeField] private RectTransform _hostGroup;
        [SerializeField] private RectTransform _playersGroup;

        private bool _isVisible;
        private Lobby _lobby;
        private string _hostName;

        public void SetVisibility(bool value)
        {
            if (_isVisible == value)
                return;
            
            _isVisible = value;
            if (_isVisible) Show(0.35f);
            else Hide();
        }

        public void Initialize(Action<Lobby> OnJoinLobby) => 
            _joinButton.onClick.AddListener(() => OnJoinLobby?.Invoke(_lobby));

        public override void Show(float? speed = null)
        {
            float realSpeed = speed ?? _duration;
            CanvasGroup.Null()?.ShowCanvasGroup(speed ?? _duration);
            if (Canvas != null)
                Canvas.sortingOrder = SortOrderOnOpen;

            _mySequence?.Kill();
            _mySequence = DOTween.Sequence();
            _mySequence.Append(_rectTransform.DOSizeDelta(_openedDimension, realSpeed));
            _mySequence.Play().OnComplete(() => OnCompletePlay(realSpeed));
        }

        public override void Hide()
        {
            CanvasGroup.Null()?.HideCanvasGroup(_duration);
            if (Canvas != null)
                Canvas.sortingOrder = 1;

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<CanvasGroup>().Null()?.HideCanvasGroup(0);

            _mySequence?.Kill();
            _mySequence = DOTween.Sequence();
            _mySequence.Append(_rectTransform.DOSizeDelta(_originalDimension, _duration));
            _mySequence.Play();
        }

        public void DisplayLobbyDescription(Lobby lobby)
        {
            if (lobby.Players[0].Data["PlayerName"].Value == _hostName)
            {
                SetVisibility(false);
                _hostName = string.Empty;
                return;
            }
            
            SetVisibility(true);
            _lobby = lobby;
            RemoveLobbyPlayerDescriptions();
            
            List<Player> players = lobby.Players;
            _hostName = players[0].Data["PlayerName"].Value;
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
    }
}