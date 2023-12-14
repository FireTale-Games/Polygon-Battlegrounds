using UnityEngine;

namespace FTS.Data.Map
{
    [CreateAssetMenu(fileName = "Map Item", menuName = "FTS/Items/Map Item")]
    internal sealed class GameMap : Item
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}