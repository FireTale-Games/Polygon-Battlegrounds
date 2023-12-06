using FTS.Tools.ExtensionMethods;
using FTS.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FTS.UI
{
    [DisallowMultipleComponent]
    internal sealed class MenuButtonController : MonoBehaviour
    {
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _hoveredColor;
        
        [SerializeField] private GameObject _previousButtonObject;

        private void OnEnter(IMenuButtonUi menuButton) => 
            menuButton.SetTextColor(menuButton == _previousButtonObject.Null()?.GetComponent<IMenuButtonUi>() ? _selectedColor : _hoveredColor);

        private void OnExit(IMenuButtonUi menuButton) => 
            menuButton.SetTextColor(menuButton == _previousButtonObject.Null()?.GetComponent<IMenuButtonUi>() ? _selectedColor : _defaultColor);
        
        private void ScreenChange(IScreen screen = null)
        {
            if (screen == null)
            {
                EventSystem.current.SetSelectedGameObject(_previousButtonObject);
                _previousButtonObject.GetComponent<IMenuButtonUi>().SetTextColor(_hoveredColor);
                _previousButtonObject = null;
                return;
            }

            if (_previousButtonObject == null)
                _previousButtonObject = EventSystem.current.currentSelectedGameObject;
            
            EventSystem.current.SetSelectedGameObject(screen.ButtonObject);
        }

        private void OnEnable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter += OnEnter;
            _controller.OnExit += OnExit;
            _controller.OnScreenChange += ScreenChange;
        }

        private void OnDisable()
        {
            IMenuController<IMenuButtonUi> _controller = GetComponent<IMenuController<IMenuButtonUi>>();
            _controller.OnEnter -= OnEnter;
            _controller.OnExit -= OnExit;
            _controller.OnScreenChange -= ScreenChange;
        }
    }
}