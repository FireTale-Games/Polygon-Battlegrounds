using System;
using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class MultiplayerScreen : MenuScreenBase
    {
        private Action OnProfileShow;
        
        protected override void OnInitialize(IManager manager)
        {
            if (manager is not ProfileManager profileManager)
                return;

            IProfile[] profiles = GetComponentsInChildren<IProfile>();
            profileManager.SetInitialValues(profiles);
            OnProfileShow = () => profileManager.RefreshValues(profiles);
        }

        public override void Show(float? speed = null)
        {
            base.Show(speed);
            OnProfileShow?.Invoke();
        }
    }
}