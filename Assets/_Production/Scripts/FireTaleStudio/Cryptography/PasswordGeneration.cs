using System;
using UnityEngine;

namespace FTS.Cryptography
{
    internal sealed class PasswordGeneration
    {
        private static string GeneratePassword(Type type)
        {
            string typeName = type.Name;
            string namespaceName = type.Namespace ?? "";
            string assemblyName = type.Assembly.GetName().Name;
            int hash = Animator.StringToHash(typeName);
            const string appSpecificConstant = "Polygon-Battlegrounds";
            
            return $"{typeName}-{namespaceName}-{assemblyName}-{hash}-{appSpecificConstant}";
        }

        public string GetPassword(Type type) => 
            new(GeneratePassword(type));
    }
}