using FTS.UI.Screens;
using UnityEngine;

namespace FTS.UI
{
    public class MainMenuUiController : MonoBehaviour, IButtonHandler<IMenuButtonUi>
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoveredColor;
        [SerializeField] private Color _selectedColor;
        
        [SerializeField] private ScreenBase _startingScreen;
        private IScreen _currentScreen;

        private void Awake() => 
            _currentScreen = _startingScreen.GetComponent<IScreen>();

        public void OnEnter(IMenuButtonUi button) =>
            button.SetTextColor(_hoveredColor);

        public void OnExit(IMenuButtonUi button) =>
            button.SetTextColor(_defaultColor);

        public void OnClick(IMenuButtonUi button) =>
            button.OnInteract(_selectedColor);

        private void ShowScreen()
        {
            
        }

        private void HideScreen()
        {
            
        }
    }
}