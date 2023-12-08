using FTS.Data;
using UnityEngine;
using PlayerSettings = FTS.Data.PlayerSettings;

namespace FTS.Managers
{
    [DisallowMultipleComponent]
    internal sealed class MenuPlayManager : BaseManager
    {
        [field: SerializeField] public GameSettings GameSettings { get; private set; } = new();
        public void SetGameType(GameType type) => GameSettings.SetGameType(type);
        public void SetPlayerSettings(PlayerSettings playerSettings) => GameSettings.SetPlayerSettings(playerSettings);
        public void SetLobbySettings(LobbySettings lobbySettings) => GameSettings.SetLobbySettings(lobbySettings);
        public void SetMapSettings(MapSettings mapSettings) => GameSettings.SetMapSettings(mapSettings);
    }
}