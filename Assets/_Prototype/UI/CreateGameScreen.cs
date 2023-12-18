using DG.Tweening;
using FTS.Data;
using FTS.Data.Map;
using FTS.Managers;
using FTS.Tools.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class CreateGameScreen : MenuScreenBase
    {
        [SerializeField] private Button _createButton;
        [SerializeField] private TMP_Dropdown _playerDropdown;
        [SerializeField] private TMP_InputField _gamePasswordField;
        [SerializeField] private TMP_InputField _gameNameInputField;
        
        private bool isVisible;
        private int _playerNumber = 2;
        private string _lobbyPassword = string.Empty;
        private string _gameName = "";

        protected override void BindToLobbyManager(LobbyManager lobbyManager)
        {
            _playerDropdown.onValueChanged.AddListener(value => _playerNumber = value + 2);
            _gamePasswordField.onValueChanged.AddListener(value => _lobbyPassword = value);
            _gameNameInputField.onValueChanged.AddListener(value => _gameName = value);
            _createButton.onClick.AddListener(() => SetLobbyData(lobbyManager));
            _createButton.onClick.AddListener(OnCreateLobby);
            _createButton.onClick.AddListener(SetVisibility);

            return;
            async void OnCreateLobby() => await lobbyManager.CreateLobby();
        }

        private void SetLobbyData(LobbyManager lobbyManager)
        {
            GameMap gameMap = ItemDatabase.GetAllOfType<GameMap>()[0];
            MapData mapData = new(0.5f, 1.0f, 1.0f, 1.0f, 20.0f, true);
            lobbyManager.SetMapSettings(new MapSettings(gameMap.Id, mapData));
            lobbyManager.SetLobbySettings(new LobbySettings(
                _gameName = _gameName.Length <= 0 ? _gameName.GenerateRandomString(10) : _gameName, _playerNumber, 
                _lobbyPassword));
        }
        
        public void SetVisibility()
        {
            isVisible = !isVisible;
            if (isVisible) Show(0.35f);
            else Hide(0.35f);
        }

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

        public override void Hide(float? speed = null)
        {
            float realSpeed = speed ?? _duration;
            CanvasGroup.Null()?.HideCanvasGroup(speed ?? _duration);
            if (Canvas != null)
                Canvas.sortingOrder = 1;
            
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).GetComponent<CanvasGroup>().Null()?.HideCanvasGroup(0);
            
            _mySequence?.Kill();
            _mySequence = DOTween.Sequence();
            _mySequence.Append(_rectTransform.DOSizeDelta(_originalDimension, realSpeed));
            _mySequence.Play();
        }
    }
}