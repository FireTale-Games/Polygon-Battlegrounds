using UnityEngine;

namespace FTS.Data
{
    internal abstract class Item : ItemBase
    {
        [field: SerializeField] public string DisplayName { get; protected set; }
    }
}