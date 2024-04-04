using System.Collections.Generic;
using MoonriseGames.Toolbox.Localization;
using UnityEditor;

namespace MoonriseGames.Toolbox.Editor.Inspectors
{
    [CustomEditor(typeof(TranslationSprites))]
    public class TranslationSpritesInspector : UnityEditor.Editor
    {
        private SerializedProperty Translations { get; set; }

        private void OnEnable()
        {
            Translations = serializedObject.FindProperty("_translations");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawInfoBox();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawInfoBox()
        {
            var languages = GetAvailableLanguages();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox($"Languages: {languages}", MessageType.Info);
        }

        private string GetAvailableLanguages()
        {
            if (Translations.arraySize == 0)
                return "None";

            var languages = new List<string>();

            for (var i = 0; i < Translations.arraySize; i++)
            {
                var value = Translations.GetArrayElementAtIndex(i);
                var languageProperty = value.FindPropertyRelative("_language");
                var languageName = languageProperty.enumDisplayNames[languageProperty.enumValueIndex];

                languages.Add(languageName);
            }

            return string.Join(", ", languages);
        }
    }
}
