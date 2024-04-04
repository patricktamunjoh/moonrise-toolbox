using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Localization;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringPropertyDrawer : DividedPropertyDrawer
    {
        private GUIStyle SuggestionStyle =>
            new()
            {
                padding = GUI.skin.textField.padding,
                normal = { textColor = new Color(1, 1, 1, .35f) },
                alignment = TextAnchor.MiddleRight
            };

        private SerializedProperty Key { get; set; }

        private string Translation { get; set; }
        private string Suggestion { get; set; }

        protected override void OnPrepareDraw(SerializedProperty property, out bool isDrawValid)
        {
            Key = property.FindPropertyRelative("_key");
            isDrawValid = true;

            try
            {
                Translation = LocalizationService.Unit.GetLocalization(Key.stringValue, string.Empty);
                Suggestion = LocalizationService.Unit.GetClosestKey(Key.stringValue, string.Empty);
            }
            catch (SingletonNotAvailableException)
            {
                Translation = string.Empty;
                Suggestion = string.Empty;
            }
        }

        protected override void OnDrawLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            var translation = Translation.Clamped((int)position.width / 7 - label.text.Length, "...");

            if (string.IsNullOrEmpty(translation).Not())
                translation = " - " + translation;

            EditorGUI.LabelField(position, new GUIContent(label.text + translation, tooltip: Translation));
        }

        protected override void OnDrawContent(Rect position, SerializedProperty property)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab && string.IsNullOrEmpty(Suggestion).Not())
                Key.stringValue = Suggestion;

            EditorGUI.PropertyField(position, Key, new GUIContent());
            EditorGUI.LabelField(position, Suggestion, SuggestionStyle);
        }
    }
}
