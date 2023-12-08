using System;
using FTS.Data;
using FTS.Managers;
using FTS.Tools.Extensions;
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
        [SerializeField] private MenuButtonUi _nextButton;
        [SerializeField] private MenuScreenBase[] _menuScreenBases;
        
        private Func<int?> OnGetProfile;
        private Func<GameType> OnGetGameType;
        private Action<int> OnProfileSet;

        protected override void OnInitialize(IManager manager)
        {
            if (manager is ProfileManager profileManager)
                BindToProfileManager(profileManager);

            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToProfileManager(ProfileManager profileManager)
        {
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

                int? invoke = OnGetProfile?.Invoke();
                OnProfileSet?.Invoke(invoke!.Value);
            }

            int? GetProfile() => 
                profileManager.GetProfile;
        }
        
        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            OnGetGameType += GetGameType;
            OnProfileSet = value => menuPlayManager.SetPlayerSettings(new PlayerSettings(value));
            
            return;
            GameType GetGameType() => 
                menuPlayManager.GameSettings.GameType;
        }
        
        protected override void OnCompletePlay(float speed)
        {
            base.OnCompletePlay(speed);
            int? profile = OnGetProfile.Invoke();
            if (profile.HasValue)
            {
                OnProfileSet?.Invoke(profile.Value);
                return;
            }
            
            _characterProfileCanvas.gameObject.SetActive(false);
            _createProfileObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(_createProfileObject.GetComponentInChildren<TMP_InputField>().gameObject);
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            _nextButton.SetBaseScreen(OnGetGameType?.Invoke() == GameType.Singleplayer ? _menuScreenBases[0] : _menuScreenBases[1]);
        }
    }
}