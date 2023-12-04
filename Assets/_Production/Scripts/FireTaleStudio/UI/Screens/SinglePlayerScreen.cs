using System;
using FTS.Managers;
using FTS.UI.Profiles;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerScreen : MenuScreenBase, ISMScreen
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private MenuScreenBase _characterCreationScreen;
        private Action OnProfileShow;
        
        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.OnInitialize += OnInitialize;
            //_playButton.onClick.AddListener(() => FindObjectOfType<MenuPlayManager>().StartGame());
        }

        private void OnDestroy() => 
            GameManager.Instance.OnInitialize -= OnInitialize;

        private void OnInitialize(IManager manager)
        {
            if (manager is ProfileManager profileManager)
                OnProfileShow = () => profileManager.SetInitialValues(GetComponentsInChildren<IProfile>());
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnProfileShow?.Invoke();
        }

        public void CreateNewProfile() => 
            _characterCreationScreen.Show();
    }

    internal interface ISMScreen
    {
        public void CreateNewProfile();
    }
}