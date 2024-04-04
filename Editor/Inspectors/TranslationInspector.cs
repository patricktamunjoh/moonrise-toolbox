using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Editor.Constants;
using MoonriseGames.Toolbox.Editor.Utilities;
using MoonriseGames.Toolbox.Extensions;
using UnityEditor;
using UnityEngine;

namespace MoonriseGames.Toolbox.Editor.Inspectors
{
    [CustomEditor(typeof(Localization.Translation))]
    public class TranslationInspector : UnityEditor.Editor
    {
        private SerializedProperty Delimiter { get; set; }
        private SerializedProperty Escape { get; set; }
        private SerializedProperty Comment { get; set; }

        private SerializedProperty TranslationMap { get; set; }
        private SerializedProperty LanguageFiles { get; set; }

        private void OnEnable()
        {
            Delimiter = serializedObject.FindProperty("_delimiter");
            Escape = serializedObject.FindProperty("_escape");
            Comment = serializedObject.FindProperty("_comment");

            TranslationMap = serializedObject.FindProperty("_translationMap");
            LanguageFiles = serializedObject.FindProperty("_languageFiles");
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawSpecialCharacterProperties();
            DrawLanguageFileProperty();

            serializedObject.ApplyModifiedProperties();

            DrawInfoBox();
            DrawRefreshButton();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawLanguageFileProperty()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(LanguageFiles);
        }

        private void DrawSpecialCharacterProperties()
        {
            var rect = EditorGUILayout.GetControlRect(false);
            var rects = rect.DivideHorizontal(0, EditorGUIUtility.labelWidth, null, null, null).ToArray();

            EditorGUI.LabelField(rects[0], "Special Characters");

            EditorLayoutingUtility.PropertyFieldWithDynamicLabelWidth(rects[1].Inset(left: Sizes.SpacingHorizontalSmall), Delimiter);
            EditorLayoutingUtility.PropertyFieldWithDynamicLabelWidth(rects[2].Inset(left: Sizes.SpacingHorizontal), Escape);
            EditorLayoutingUtility.PropertyFieldWithDynamicLabelWidth(rects[3].Inset(left: Sizes.SpacingHorizontal), Comment);
        }

        private void DrawRefreshButton()
        {
            EditorGUILayout.Space();

            if (GUILayout.Button("Refresh Language Files"))
                ((Localization.Translation)target).RefreshLanguageData();
        }

        private void DrawInfoBox()
        {
            var itemCount = GetKeyCount();
            var languages = GetAvailableLanguages();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox($"Languages: {languages}\nKeys: {itemCount}", MessageType.Info);
        }

        private string GetAvailableLanguages()
        {
            TranslationMap.serializedObject.Update();
            var languagesProperty = TranslationMap.FindPropertyRelative("_languages");

            if (languagesProperty.arraySize == 0)
                return "None";

            var languages = new List<string>();

            for (var i = 0; i < languagesProperty.arraySize; i++)
            {
                var language = languagesProperty.GetArrayElementAtIndex(i);
                var languageName = language.enumDisplayNames[language.enumValueIndex];

                languages.Add(languageName);
            }

            return string.Join(", ", languages);
        }

        private int GetKeyCount()
        {
            TranslationMap.serializedObject.Update();
            var setsProperty = TranslationMap.FindPropertyRelative("_sets");
            return setsProperty.arraySize;
        }
    }
}
