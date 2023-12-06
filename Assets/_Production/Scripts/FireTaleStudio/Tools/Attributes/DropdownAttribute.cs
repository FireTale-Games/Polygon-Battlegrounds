using System;
using UnityEngine;

namespace FTS.Tools.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DropdownAttribute : PropertyAttribute
    {
        public string MethodName { get; private set; }

        public DropdownAttribute(string methodName) => 
            MethodName = methodName;
    }
}