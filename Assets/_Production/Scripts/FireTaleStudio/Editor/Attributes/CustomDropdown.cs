using System;
using System.Reflection;
using FTS.Tools.Attributes;
using UnityEditor;
using UnityEngine;

namespace FTS.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the attribute to find the method name
            DropdownAttribute dropdownAttribute = attribute as DropdownAttribute;
            string methodName = dropdownAttribute?.MethodName;

            // Use reflection to call the method to get scene names
            MethodInfo getSceneNamesMethod = property.serializedObject.targetObject.GetType()
                .GetMethod(methodName ?? throw new InvalidOperationException("Method name is null!"), BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (getSceneNamesMethod == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            string[] sceneOptions = getSceneNamesMethod.Invoke(property.serializedObject.targetObject, null) as string[];

            if (sceneOptions == null)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            // Find the index of the currently selected scene
            int currentIndex = Array.IndexOf(sceneOptions, property.stringValue);
            if (currentIndex < 0) currentIndex = 0;

            // Draw the popup to select a scene
            int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, sceneOptions);

            // Update the property with the selected scene
            if (selectedIndex < 0 || selectedIndex >= sceneOptions.Length) 
                return;
            
            property.stringValue = sceneOptions[selectedIndex];
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}