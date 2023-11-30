using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerScreen : MenuScreenBase
    {
        public override void Show(float? speed = null)
        {
            base.Show(speed);
            FindObjectOfType<ProfileManager>().SetInitialValues(GetComponentsInChildren<IProfile>());
        }
    }
}