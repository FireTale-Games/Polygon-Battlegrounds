using FTS.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [SerializeField] private Button _backButton;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is not MultiplayerManager multiplayerManager)
                return;
            
            _backButton.onClick.AddListener(() => multiplayerManager.SetNetworkConnection(false));
        }
    }
}