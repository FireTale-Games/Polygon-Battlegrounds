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
        public readonly string r_playerNumber;
        public readonly Action<Lobby> r_onJoinLobby;

        internal LobbyGameUiData(Sprite lockSprite, string mapName, string playerNumber, Action<Lobby> onJoinLobby)
        {
            r_lockSprite = lockSprite;
            r_mapName = mapName;
            r_playerNumber = playerNumber;
            r_onJoinLobby = onJoinLobby;
        }
    }
    
    internal sealed class LobbyGameUi : MonoBehaviour, ILobbyGameUi
    {
        [SerializeField] private Image _lockImage;
        [SerializeField] private TextMeshProUGUI _mapNameLabel;
        [SerializeField] private TextMeshProUGUI _playerNumberLabel;
        [SerializeField] private Button _joinButton;
        
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby)
        {
            _lockImage.sprite = lobbyGameUiData.r_lockSprite;
            _mapNameLabel.text = lobbyGameUiData.r_mapName;
            _playerNumberLabel.text = lobbyGameUiData.r_playerNumber;
            _joinButton.onClick.AddListener(() => lobbyGameUiData.r_onJoinLobby?.Invoke(lobby));
        }

        private void OnDestroy() => 
            _joinButton.onClick.RemoveAllListeners();
    }

    internal interface ILobbyGameUi
    {
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby);
    }
}