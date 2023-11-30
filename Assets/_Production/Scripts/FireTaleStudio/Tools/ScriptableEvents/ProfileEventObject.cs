using FTS.UI.Profiles;
using UnityEngine;

namespace FTS.Tools.ScriptableEvents
{
    [CreateAssetMenu(fileName = "New Profile Event", menuName = "Events/New Profile Event")]
    internal sealed class ProfileEventObject : EventInvoker<IProfile>
    {

    }
}