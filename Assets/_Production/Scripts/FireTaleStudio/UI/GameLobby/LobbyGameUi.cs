using System;
using FTS.UI.Screens;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.GameLobby
{
    internal sealed class LobbyGameUi : MonoBehaviour, ILobbyGameUi
    { 
        [SerializeField] private Image _lockImage;
        [SerializeField] private TextMeshProUGUI _lobbyName;
        [SerializeField] private TextMeshProUGUI _playerNumberLabel; 
        [SerializeField] private Button _joinButton;
        
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby)
        {
            _lockImage.sprite = lobbyGameUiData.r_lockSprite;
            _lobbyName.text = lobby.Name;
            _playerNumberLabel.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}"; _joinButton.onClick.AddListener(() => lobbyGameUiData.r_onJoinLobby?.Invoke(lobby));
            _joinButton.GetComponent<MenuButtonUi>().SetBaseScreen(lobbyGameUiData.r_menuScreenBase);
        }
    }

    internal interface ILobbyGameUi
    {
        public void Initialize(LobbyGameUiData lobbyGameUiData, Lobby lobby);
    }
    
    internal readonly struct LobbyGameUiData
    {
        public readonly Sprite r_lockSprite;
        public readonly Action<Lobby> r_onJoinLobby;
        public readonly MenuScreenBase r_menuScreenBase;

        internal LobbyGameUiData(Sprite lockSprite, Action<Lobby> onJoinLobby, MenuScreenBase menuScreenBase)
        {
            r_lockSprite = lockSprite;
            r_onJoinLobby = onJoinLobby;
            r_menuScreenBase = menuScreenBase;
        }
    }
}