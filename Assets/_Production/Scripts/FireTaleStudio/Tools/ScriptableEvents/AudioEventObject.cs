using UnityEngine;

namespace FTS.Tools.ScriptableEvents
{
    [CreateAssetMenu(fileName = "New Audio Event", menuName = "Events/New Audio Event")]
    internal sealed class AudioEventObject : EventInvoker<AudioClip>
    {

    }
}
