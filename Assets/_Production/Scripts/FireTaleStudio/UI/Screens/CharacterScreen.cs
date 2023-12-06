using System;
using FTS.Managers;
using FTS.Tools.ExtensionMethods;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class CharacterScreen : MenuScreenBase
    {
        [SerializeField] private TextMeshProUGUI _createProfileText;
        [SerializeField] private GameObject _createProfileObject;
        [SerializeField] private CanvasGroup _characterProfileCanvas;
        [SerializeField] private Button _createProfileButton;

        private Func<bool> OnGetProfile;

        protected override void OnInitialize(IManager manager)
        {
            if (manager is not ProfileManager profileManager)
                return;
            
            OnGetProfile += GetProfile;

            _createProfileButton.onClick.AddListener(CreateProfileButtonBind);

            return;
            void CreateProfileButtonBind()
            {
                if (_createProfileText.text.Length <= 3)
                    return;
                
                profileManager.CreateNewProfile(_createProfileText.text);
                _createProfileObject.SetActive(false);
                _characterProfileCanvas.gameObject.SetActive(true);
                _characterProfileCanvas.ShowCanvasGroup(0.35f);
                EventSystem.current.SetSelectedGameObject(_characterProfileCanvas.GetComponentInChildren<Button>().gameObject);
            }

            bool GetProfile() => 
                profileManager.GetProfile.HasValue;
        }

        protected override void OnCompletePlay(float speed)
        {
            base.OnCompletePlay(speed);
            if (OnGetProfile.Invoke())
                return;
            
            _characterProfileCanvas.gameObject.SetActive(false);
            _createProfileObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_createProfileObject.GetComponentInChildren<TMP_InputField>().gameObject);
        }
    }
}