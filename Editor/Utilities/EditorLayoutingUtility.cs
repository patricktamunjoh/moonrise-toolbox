using MoonriseGames.Toolbox.Editor.Constants;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.Utilities
{
    public static class EditorLayoutingUtility
    {
        public static void PropertyFieldWithDynamicLabelWidth(Rect position, SerializedProperty property, GUIContent label = null)
        {
            label ??= new GUIContent(property.displayName);

            var labelWidth = EditorStyles.label.CalcSize(label).x;
            var defaultLabelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = labelWidth + Sizes.SpacingHorizontal;
            EditorGUI.PropertyField(position, property, label);
            EditorGUIUtility.labelWidth = defaultLabelWidth;
        }
    }
}
