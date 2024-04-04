using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Organization;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.Inspectors
{
    [InitializeOnLoad]
    internal static class HierarchyMarkerInspector
    {
        private const string BACKGROUND_SEPARATOR = "#383838";
        private const string BACKGROUND_SEPARATOR_DARK = "#313131";

        private const string TEXT_DARK = "#191919";

        static HierarchyMarkerInspector()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect rect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);

            var marker = (obj as GameObject)?.GetComponent(typeof(HierarchyMarker)) as HierarchyMarker;

            if (marker == null || marker.transform.childCount > 0)
                return;

            DrawBackground(marker, rect);
            DrawSeparatorBackground(marker, rect);

            DrawText(marker, rect);
            DrawSeparator(marker, rect);
        }

        private static void DrawSeparatorBackground(HierarchyMarker marker, Rect rect)
        {
            if (marker.IsSeparator.Not())
                return;

            var rectDark = new Rect(0, rect.y, 32, rect.height);
            rect = new Rect(32, rect.y, rect.width + rect.x, rect.height);

            ColorUtility.TryParseHtmlString(BACKGROUND_SEPARATOR, out var color);
            ColorUtility.TryParseHtmlString(BACKGROUND_SEPARATOR_DARK, out var colorDark);

            EditorGUI.DrawRect(rect, color);
            EditorGUI.DrawRect(rectDark, colorDark);
        }

        private static void DrawBackground(HierarchyMarker marker, Rect rect)
        {
            if (marker.IsSeparator)
                return;

            var isSelected = Selection.activeObject == marker.gameObject;
            var isHovered = rect.Contains(Event.current.mousePosition);

            var color = marker.Color.WithAlpha(1);

            if (isSelected)
                color = new Color(color.r + .125f, color.g + .125f, color.b + .125f);

            if (isSelected.Not() && isHovered)
                color = new Color(color.r + .075f, color.g + .075f, color.b + .075f);

            rect = new Rect(32, rect.y, rect.width + rect.x, rect.height);

            EditorGUI.DrawRect(rect, color);
        }

        private static void DrawText(HierarchyMarker marker, Rect rect)
        {
            if (marker.IsSeparator)
                return;

            var textColor = Color.white;

            if (Mathf.Max(marker.Color.r, marker.Color.g, marker.Color.b) > .65f)
                ColorUtility.TryParseHtmlString(TEXT_DARK, out textColor);

            var style = new GUIStyle
            {
                normal = new GUIStyleState { textColor = textColor },
                fontStyle = FontStyle.Bold
            };

            EditorGUI.LabelField(rect.Inset(left: 18, right: 18), marker.name, style);
        }

        private static void DrawSeparator(HierarchyMarker marker, Rect rect)
        {
            if (marker.IsSeparator.Not())
                return;

            var style = new GUIStyle
            {
                normal = new GUIStyleState { textColor = Color.white },
                fontStyle = FontStyle.Normal
            };

            var rectEndOverlay = new Rect(rect.x + rect.width - 18, rect.y, 100, rect.height);

            ColorUtility.TryParseHtmlString(BACKGROUND_SEPARATOR, out var color);

            EditorGUI.LabelField(rect, new string('⸺', 100), style);
            EditorGUI.DrawRect(rectEndOverlay, color);
        }
    }
}
