using TMPro;
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
        [SerializeField] private LobbyGameUi _lobbyGameUi;
        [SerializeField] private RectTransform _lobbyList;
        [SerializeField] private MenuButtonUi _createLobby;
        [SerializeField] private Button _refreshLobby;

        [Header("Description Data"), Space(2)]
        [SerializeField] private LobbyPlayerDescriptionUi _lobbyPlayerDescriptionUi;
        [SerializeField] private RectTransform _hostGroup;
        [SerializeField] private RectTransform _playersGroup;
    }
}