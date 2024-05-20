using System;
using System.Reflection;
using MoonriseGames.Toolbox.Editor.Constants;
using MoonriseGames.Toolbox.Extensions;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.PropertyDrawers
{
    public abstract class DividedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty property, GUIContent label)
        {
            OnPrepareDraw(property, out var isDrawValid);

            if (isDrawValid.Not())
            {
                EditorGUI.BeginProperty(pos, label, property);
                EditorGUI.LabelField(pos, label.text, GetErrorMessage(property));
                return;
            }

            pos = GetFinalPosition(pos, property);
            label = EditorGUI.BeginProperty(new Rect(pos.x - 500, pos.y, pos.width + 50, pos.height), label, property);

            var labelPosition = new Rect(pos.x, pos.y, Sizes.WidthLabel, pos.height);
            OnDrawLabel(labelPosition, property, label);

            var contentWidth = Sizes.WidthLabel + Sizes.SpacingHorizontalSmall;
            var contentPosition = new Rect(pos.x + contentWidth, pos.y, pos.width - contentWidth, pos.height);
            OnDrawContent(contentPosition, property);

            EditorGUI.EndProperty();
        }

        protected virtual string GetErrorMessage(SerializedProperty property) => throw new NotImplementedException();

        protected virtual Rect GetFinalPosition(Rect position, SerializedProperty property) => position;

        protected abstract void OnPrepareDraw(SerializedProperty property, out bool isDrawValid);

        protected abstract void OnDrawLabel(Rect position, SerializedProperty property, GUIContent label);

        protected abstract void OnDrawContent(Rect position, SerializedProperty property);
    }
}
