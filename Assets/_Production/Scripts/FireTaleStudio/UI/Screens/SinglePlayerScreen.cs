using FTS.Managers;
using FTS.UI.Profiles;

namespace FTS.UI.Screens
{
    internal sealed class SinglePlayerScreen : MenuScreenBase
    {
        private void Start() => 
            FindObjectOfType<ProfileManager>().SetInitialValues(GetComponentsInChildren<IProfile>());
    }
}