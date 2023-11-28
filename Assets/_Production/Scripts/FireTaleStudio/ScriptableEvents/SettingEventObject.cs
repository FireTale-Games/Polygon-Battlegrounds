using FTS.UI.Settings;
using UnityEngine;

namespace FTS.Tools.ScriptableEvents
{
    [CreateAssetMenu(fileName = "New Setting Event", menuName = "Events/New Setting Event")]
    public class SettingEventObject : EventInvoker<ISetting>
    {

    }
}