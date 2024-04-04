using System.Linq;
using MoonriseGames.Toolbox.Editor.Constants;
using MoonriseGames.Toolbox.Editor.Utilities;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Types;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Interval))]
    public class IntervalPropertyDrawer : DividedPropertyDrawer
    {
        private SerializedProperty Start { get; set; }
        private SerializedProperty End { get; set; }

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
            var rects = position.DivideHorizontal(0, 3).ToArray();
            var isValueMismatch = Start.hasMultipleDifferentValues || End.hasMultipleDifferentValues;

            using (new EditorGUI.DisabledScope(isValueMismatch))
            {
                EditorLayoutingUtility.PropertyFieldWithDynamicLabelWidth(rects[0], Start);

                if (isValueMismatch.Not())
                    Start.floatValue = Mathf.Min(Start.floatValue, End.floatValue);

                EditorLayoutingUtility.PropertyFieldWithDynamicLabelWidth(rects[1].Inset(left: Sizes.SpacingHorizontal), End);

                if (isValueMismatch.Not())
                    End.floatValue = Mathf.Max(End.floatValue, Start.floatValue);
            }
        }
    }
}
