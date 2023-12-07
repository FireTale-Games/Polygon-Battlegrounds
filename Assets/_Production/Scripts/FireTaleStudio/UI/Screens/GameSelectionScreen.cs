using System;
using FTS.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameSelectionScreen : MenuScreenBase
    {
        [SerializeField] private Button _playGame;
        [SerializeField] private GameObject _multiplayerOptions;

        private Func<GameType> OnGetGameType; 
        
        protected override void Awake()
        {
            base.Awake();
            _playGame.onClick.RemoveAllListeners();
            _playGame.onClick.AddListener(() => SceneManager.LoadScene("EmptyTemplate_Scene"));
        }

        protected override void OnInitialize(IManager manager)
        {
            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager)
        {
            OnGetGameType += GetGameType;
            
            return;
            GameType GetGameType() => 
                menuPlayManager.GameSettings.GameType;
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            _multiplayerOptions.SetActive(OnGetGameType?.Invoke() == GameType.Multiplayer);
        }
    }
}