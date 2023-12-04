using FTS.Managers;
using FTS.UI.Profiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class CharacterScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _createButton;

        private IProfile _currentProfile;
        
        private void Awake() => 
            GameManager.Instance.OnInitialize += OnInitialize;

        public void Show(IProfile profile)
        {
            _currentProfile = profile;
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            _currentProfile = null;
            gameObject.SetActive(false);
        }
        
        private void OnInitialize(IManager manager)
        {
            if (manager is not ProfileManager profileManager)
                return;
            
            _backButton.onClick.AddListener(() => gameObject.SetActive(false));
            _createButton.onClick.AddListener(() =>
            {
                if (_text.text.Length < 3)
                    return;
                
                profileManager.CreateNewProfile(_currentProfile, _text.text);
                Hide();
            });
            
            Hide();
        }
    }
}