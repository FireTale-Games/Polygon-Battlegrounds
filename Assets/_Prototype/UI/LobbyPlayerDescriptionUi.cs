using TMPro;
using UnityEngine;

namespace FTS.UI
{
    internal sealed class LobbyPlayerDescriptionUi : MonoBehaviour, ILobbyPlayerDescriptionUi
    {
        [SerializeField] private TextMeshProUGUI _playerNameLabel;
        [SerializeField] private TextMeshProUGUI _playerTypeLabel;
        
        public void Initialize(string playerName, string playerType)
        {
            _playerNameLabel.text = playerName;
            _playerTypeLabel.text = playerType;
        }

        public void Destroy() => Destroy(gameObject);
    }

    internal interface ILobbyPlayerDescriptionUi
    {
        public void Initialize(string playerName, string playerType);
        public void Destroy();
    }
}