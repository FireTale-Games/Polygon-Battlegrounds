using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    internal readonly struct LobbyGameUiData
    {
        public readonly Sprite r_lockSprite;
        public readonly string r_mapName;
        public readonly int r_playerNumber;
        public readonly int r_playerMaxNumber;
        public readonly Action<Lobby> r_onSelectLobby;
        public readonly Action<Lobby> r_onJoinLobby;

        internal LobbyGameUiData(Sprite lockSprite, string mapName, int playerNumber, int playerMaxNumber, Action<Lobby> onJoinLobby, Action<Lobby> onSelectLobby)
        {
            r_lockSprite = lockSprite;
            r_mapName = mapName;
            r_playerNumber = playerNumber;
            r_playerMaxNumber = playerMaxNumber;
            r_onSelectLobby = onSelectLobby;
            r_onJoinLobby = onJoinLobby;
        }
    }
    
    internal sealed class LobbyGameUi : MonoBehaviour, ILobbyGameUi
    {
        [SerializeField] private Image _lockImage;
        [SerializeField] private TextMeshProUGUI _lobbyName;
        [SerializeField] private TextMeshProUGUI _playerNumberLabel;
        [SerializeField] private Button _selectLobby;
        [SerializeField] private Button _joinButton;
        
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby)
        {
            _lockImage.sprite = lobbyGameUiData.r_lockSprite;
            _lobbyName.text = lobbyGameUiData.r_mapName;
            _playerNumberLabel.text = $"{lobbyGameUiData.r_playerNumber}/{lobbyGameUiData.r_playerMaxNumber}";
            _selectLobby.onClick.AddListener(() => lobbyGameUiData.r_onSelectLobby?.Invoke(lobby));
            _joinButton.onClick.AddListener(() => lobbyGameUiData.r_onJoinLobby?.Invoke(lobby));
        }

        private void OnDestroy()
        {
            _selectLobby.onClick.RemoveAllListeners();
            _joinButton.onClick.RemoveAllListeners();
        }
    }

    internal interface ILobbyGameUi
    {
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby);
    }
}