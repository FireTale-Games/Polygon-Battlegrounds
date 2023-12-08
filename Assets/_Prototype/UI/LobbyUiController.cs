using System.Collections.Generic;
using FTS.Data;
using UnityEngine;

namespace FTS.UI
{
    public class LobbyUiController : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private PlayerUi _playerUi;
        [SerializeField] private Transform _playerList;
        
        private void Awake()
        {
            for (int i = 0; i < _gameSettings.LobbySettings.r_playerNumber; i++)
                Instantiate(_playerUi, _playerList);

            AddPlayer();
        }

        private void AddPlayer()
        {
            IPlayerUi[] _playerSlots = _playerList.GetComponentsInChildren<IPlayerUi>();
            foreach (IPlayerUi playerSlot in _playerSlots)
            {
                if (playerSlot.SlotOccupied)
                    continue;
                
                playerSlot.InitializeValues(Loader.GetPlayerData<string>(_gameSettings.PlayerSettings.r_playerName, _gameSettings.PlayerSettings.r_playerName), () => {});
                break;
            }
        }
    }

    public static class Loader
    {
        public static T GetPlayerData<T>(int profile, int data)
        {
            Dictionary<int, object> profileData = new DataLoader<Dictionary<int, object>, object>(profile.ToString()).LoadData();
            return (T)profileData[data];
        }
    }
}