using UnityEngine;

namespace FTS.UI
{
    [RequireComponent(typeof(MainMenuUiController)), DisallowMultipleComponent]
    internal sealed class MenuButtonController : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _hoveredColor;
        [SerializeField] private Color _selectedColor;
     
        private IMenuButtonUi _currentButton;
        
        private void OnEnter(IMenuButtonUi menuButton) => 
            menuButton.SetTextColor(_currentButton == menuButton ? _selectedColor : _hoveredColor);

        private void OnExit(IMenuButtonUi menuButton) => 
            menuButton.SetTextColor(_currentButton == menuButton ? _selectedColor : _defaultColor);
        
        private void OnPress(IMenuButtonUi menuButton) => 
            menuButton.SetTextColor(_currentButton == menuButton ? _hoveredColor : _defaultColor);
        
        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnExit += OnExit;
            _controller.OnPress += OnPress;
        }
        
        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnExit -= OnExit;
            _controller.OnPress -= OnPress;
        }
    }
}