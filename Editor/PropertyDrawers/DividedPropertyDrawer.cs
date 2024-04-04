using System;
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
            EditorGUI.BeginProperty(pos, label, property);

            OnDrawLabel(new Rect(pos.x, pos.y, Sizes.WidthLabel, pos.height), property, label);

            var contentWidth = Sizes.WidthLabel + Sizes.SpacingHorizontalSmall;
            OnDrawContent(new Rect(pos.x + contentWidth, pos.y, pos.width - contentWidth, pos.height), property);
        }

        protected virtual string GetErrorMessage(SerializedProperty property) => throw new NotImplementedException();

        protected virtual Rect GetFinalPosition(Rect position, SerializedProperty property) => position;

        protected abstract void OnPrepareDraw(SerializedProperty property, out bool isDrawValid);

        protected abstract void OnDrawLabel(Rect position, SerializedProperty property, GUIContent label);

        protected abstract void OnDrawContent(Rect position, SerializedProperty property);
    }
}
