using TMPro;
using UnityEngine;

namespace FTS.UI
{
    internal sealed class LobbyPlayerDescriptionUi : MonoBehaviour, ILobbyPlayerDescriptionUi
    {
        [SerializeField] private TextMeshProUGUI _playerNameLabel;
        [SerializeField] private TextMeshProUGUI _playerTypeLabel;
        
        public void Initialize()
        {
            Debug.Log("Initialized");
        }
    }

    internal interface ILobbyPlayerDescriptionUi
    {
        public void Initialize();
    }
}