using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace FTS.Data
{
    internal abstract class ItemBase : ScriptableObject
    {
        [field: SerializeField] public string Name { get; protected set; }
        
        public int Id => GetHashCode();
        
        public override int GetHashCode() => NameToId(Name);
        
        public override bool Equals(object other) => 
            other is ItemBase identifier && identifier.Id == Id;

        private static int NameToId(string input)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            int uniqueID = BitConverter.ToInt32(bytes, 0);
            return uniqueID;
        }
    }
}