using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class ExitScreen : MenuScreenBase
    {
        [SerializeField] private Button _exitButton;

        private void OnEnable() => 
            _exitButton.onClick.AddListener(Application.Quit);

        private void OnDisable() => 
            _exitButton.onClick.RemoveListener(Application.Quit);
    }
}