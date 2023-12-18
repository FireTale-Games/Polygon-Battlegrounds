using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FTS.Data
{
    [CreateAssetMenu(fileName = "Item Database", menuName = "FTS/Database/Item Database")]
    internal sealed class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<Item> _items;

        public static ItemDatabase Database => _database ??= Resources.Load<ItemDatabase>(nameof(ItemDatabase));
        private static ItemDatabase _database;
        
        public static Item Get(int id) => Database._items.Find(item => item.Id == id);
        public static T Get<T>(int id) where T : Item => Get(id) as T;
        public static List<T> GetAllOfType<T>() where T : Item => Database._items.OfType<T>().ToList();
    }
}