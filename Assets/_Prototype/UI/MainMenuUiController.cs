using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public class MainMenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>
    {
        [SerializeField] private ScreenBase _startingScreen;
        private IScreen _currentScreen;

        private void Awake() => 
            _currentScreen = _startingScreen.GetComponent<IScreen>();

        public void OnEnter(IMenuButtonUi t)
        {
            Debug.Log("Hello");
        }

        public void OnExit(IMenuButtonUi t)
        {
            Debug.Log("Goodbye");
        }

        public void OnClick(IMenuButtonUi t)
        {
            Debug.Log("Click");
        }
    }
}