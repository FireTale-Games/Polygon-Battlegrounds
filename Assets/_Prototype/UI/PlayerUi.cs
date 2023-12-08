using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    internal sealed class PlayerUi : MonoBehaviour, IPlayerUi
    {
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private Button _kickButton;

        public bool SlotOccupied { get; private set; }

        public void InitializeValues(string playerName, Action kickPlayer)
        {
            _playerName.text = playerName;
            _kickButton.onClick.AddListener(ResetValues);
            SlotOccupied = true;
        }

        private void ResetValues()
        {
            _playerName.text = "Empty Slot";
            _kickButton.onClick.RemoveAllListeners();
            SlotOccupied = true;
        }
    }

    internal interface IPlayerUi
    {
        public bool SlotOccupied { get; }
        public void InitializeValues(string playerName, Action kickPlayer);
    }
}