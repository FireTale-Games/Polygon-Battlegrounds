using System;
using FTS.Data;
using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerScreen : MenuScreenBase
    {
        private Action OnProfileShow;
        private Action OnSinglePlayerShow;
        
        protected override void BindToProfileManager(ProfileManager profileManager)
        {
            IProfile[] profiles = GetComponentsInChildren<IProfile>();
            profileManager.SetInitialValues(profiles);
            OnProfileShow = () => profileManager.RefreshValues(profiles);
        }

        protected override void BindToLobbyManager(Managers.LobbyManager lobbyManager) => 
            OnSinglePlayerShow = () => lobbyManager.SetGameType(GameType.Singleplayer);

        
        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnProfileShow?.Invoke();
            OnSinglePlayerShow?.Invoke();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnProfileShow = null;
            OnSinglePlayerShow = null;
        }
    }
}