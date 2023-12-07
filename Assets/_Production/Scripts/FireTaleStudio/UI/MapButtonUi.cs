using UnityEngine;
using UnityEngine.UI;

namespace FTS.UI
{
    internal sealed class MapButtonUi : MonoBehaviour, IMapButtonUi
    {
        [field: SerializeField] public Button MapButton { get; private set; }
        [field: SerializeField] public string MapName { get; private set; }
    }
    
    internal interface IMapButtonUi
    {
        public Button MapButton { get; }
        public string MapName { get; }
    }
}