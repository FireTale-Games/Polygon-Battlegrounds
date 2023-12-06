using FTS.Tools.ExtensionMethods;
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

        private void OnPress(IMenuButtonUi menuButton)
        {
            menuButton.SetTextColor(_selectedColor);
            
            if (_previousButtonObject != null)
            {
                EventSystem.current.SetSelectedGameObject(_previousButtonObject);
                _previousButtonObject.GetComponent<IMenuButtonUi>().SetTextColor(_hoveredColor);
                _previousButtonObject = null;
                return;
            }
            
            _previousButtonObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(menuButton.ButtonScreen.ButtonObject);
        }

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