using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(InspectorButton))]
    public class InspectorButtonPropertyDrawer : DividedPropertyDrawer
    {
        private SerializedProperty IsSet { get; set; }

        protected override void OnPrepareDraw(SerializedProperty property, out bool isDrawValid)
        {
            IsSet = property.FindPropertyRelative("_isSet");
            isDrawValid = true;
        }

        protected override void OnDrawLabel(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.LabelField(position, label);

        protected override void OnDrawContent(Rect position, SerializedProperty property)
        {
            if (GUI.Button(position, IsSet.boolValue ? "Enabled" : ""))
                IsSet.boolValue = IsSet.boolValue.Not();
        }
    }
}
