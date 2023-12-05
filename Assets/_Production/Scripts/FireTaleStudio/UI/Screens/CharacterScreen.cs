using System;
using FTS.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class CharacterScreen : MenuScreenBase
    {
        [SerializeField] private TextMeshProUGUI _createProfileText;
        [SerializeField] private GameObject _createProfileObject;
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
            }

            bool GetProfile() => 
                profileManager.GetProfile.HasValue;
        }

        protected override void OnCompletePlay(float speed)
        {
            base.OnCompletePlay(speed);
            if (OnGetProfile.Invoke())
            {
                Debug.Log("Have profile");
                return;
            }
            
            Debug.Log("Doesn't have profile");
            _createProfileObject.SetActive(true);
        }
    }
}