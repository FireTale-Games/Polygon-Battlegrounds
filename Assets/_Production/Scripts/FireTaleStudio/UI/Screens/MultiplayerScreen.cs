using System;
using FTS.Data;
using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class MultiplayerScreen : MenuScreenBase
    {
        private Action OnProfileShow;
        private Action OnSinglePlayerShow;
        

        protected override void BindToMenuPlayManager(MenuPlayManager menuPlayManager) => 
            OnSinglePlayerShow = () => menuPlayManager.SetGameType(GameType.Multiplayer);

        protected override void BindToProfileManager(ProfileManager profileManager)
        {
            IProfile[] profiles = GetComponentsInChildren<IProfile>();
            profileManager.SetInitialValues(profiles);
            OnProfileShow = () => profileManager.RefreshValues(profiles);
        }

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