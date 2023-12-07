using System;
using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerScreen : MenuScreenBase
    {
        private Action OnProfileShow;
        private Action OnSinglePlayerShow;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is ProfileManager profileManager)
                BindToProfileManager(profileManager);

            if (manager is MenuPlayManager menuPlayManager)
                BindToMenuPlayManager(menuPlayManager);
        }
        
        private void BindToProfileManager(ProfileManager profileManager)
        {
            IProfile[] profiles = GetComponentsInChildren<IProfile>();
            profileManager.SetInitialValues(profiles);
            OnProfileShow = () => profileManager.RefreshValues(profiles);
        }

        private void BindToMenuPlayManager(MenuPlayManager menuPlayManager) => 
            OnSinglePlayerShow = () => menuPlayManager.SetGameType(GameType.Singleplayer);

        
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