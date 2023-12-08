using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FTS.Data;
using FTS.Managers;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI.Screens
{
    internal sealed class GameScreen : MenuScreenBase
    {
        [SerializeField] private Button _backButton;
        private Func<GameSettings> OnGetGameSettings;

        public string PlayerId { get; private set; }
        public string PlayerName { get; private set; }
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is MultiplayerManager multiplayerManager)
                _backButton.onClick.AddListener(() => multiplayerManager.SetNetworkConnection(false));

            if (manager is MenuPlayManager menuPlayManager)
            {
                OnGetGameSettings = GetGameSettings;
                return;
                GameSettings GetGameSettings() => 
                    menuPlayManager.GameSettings;
            }
        }

        public override async void Show(float? speed = null)
        {
            base.Show(speed);
            await Authenticate();
        }
        
        private async Task Authenticate()
        {
            int? playerName = OnGetGameSettings?.Invoke().PlayerSettings.r_playerName;
            Dictionary<int, object> playerData = new DataLoader<Dictionary<int, object>, object>(playerName.ToString()).LoadData();
            if (playerName.HasValue)
                await Authenticate(playerData[playerName.Value] as string);
        }

        private async Task Authenticate(string playerName)
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            { 
                InitializationOptions options = new();
                options.SetProfile(new string(playerName.Where(c => c != '\u200B').ToArray()));
                await UnityServices.InitializeAsync(options);
            }

            AuthenticationService.Instance.SignedIn += () => Debug.Log("Signed in as " + AuthenticationService.Instance.PlayerId);

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                PlayerId = AuthenticationService.Instance.PlayerId;
                PlayerName = AuthenticationService.Instance.PlayerName;
            }
        }
    }
}