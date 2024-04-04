using System.Linq;
using MoonriseGames.Toolbox.Editor.Constants;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(IntervalRangeAttribute))]
    public class IntervalRangePropertyDrawer : DividedPropertyDrawer
    {
        private SerializedProperty Start { get; set; }
        private SerializedProperty End { get; set; }

        protected override string GetErrorMessage(SerializedProperty property) =>
            $"{nameof(IntervalRangeAttribute)} only supports {nameof(Interval)}";

        protected override void OnPrepareDraw(SerializedProperty property, out bool isDrawValid)
        {
            Start = property.FindPropertyRelative("_start");
            End = property.FindPropertyRelative("_end");

            isDrawValid = Start != null && End != null;
        }

        protected override void OnDrawLabel(Rect position, SerializedProperty property, GUIContent label) =>
            EditorGUI.LabelField(position, label);

        protected override void OnDrawContent(Rect position, SerializedProperty property)
        {
            var rects = position.DivideHorizontal(Sizes.SpacingHorizontal, Sizes.WidthElement, null, Sizes.WidthElement).ToArray();

            var range = attribute as IntervalRangeAttribute;
            var isValueMismatch = Start.hasMultipleDifferentValues || End.hasMultipleDifferentValues;

            var start = Start.floatValue;
            var end = End.floatValue;

            using (new EditorGUI.DisabledScope(isValueMismatch))
            {
                EditorGUI.MinMaxSlider(rects[1], GUIContent.none, ref start, ref end, range.Min, range.Max);

                if (isValueMismatch.Not())
                {
                    Start.floatValue = (float)System.Math.Round(start, 3);
                    End.floatValue = (float)System.Math.Round(end, 3);
                }

                EditorGUI.PropertyField(rects[0], Start, GUIContent.none);

                if (isValueMismatch.Not())
                    Start.floatValue = Mathf.Clamp(Start.floatValue, range.Min, Mathf.Min(range.Max, End.floatValue));

                EditorGUI.PropertyField(rects[2], End, GUIContent.none);

                if (isValueMismatch.Not())
                    End.floatValue = Mathf.Clamp(End.floatValue, Mathf.Max(range.Min, Start.floatValue), range.Max);
            }
        }
    }
}
