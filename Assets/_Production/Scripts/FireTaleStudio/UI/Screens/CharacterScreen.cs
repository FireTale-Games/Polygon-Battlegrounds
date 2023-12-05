using System;
using FTS.Managers;
using FTS.UI.Profiles;
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
        
        private Action OnCreateProfile;
        //private Func<>
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is not ProfileManager profileManager)
                return;
            
            OnCreateProfile += OnProfile;
            return;
            
            void OnProfile()
            {
                if (_createProfileText.text.Length <= 3)
                    return;
                
                profileManager.CreateNewProfile(_createProfileText.text);
            }
        }

        protected override void OnCompletePlay(float speed)
        {
            base.OnCompletePlay(speed);
            //if (_createProfileObject)
        }
    }
}