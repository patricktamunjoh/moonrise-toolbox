using System;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [ExecuteInEditMode]
    public class LocalizationService : SingletonGlobal<ILocalizationService>, ILocalizationService
    {
        private const string KEY_STORED_LANGUAGE = "LocalizationService/StoredLanguage";

        [SerializeField]
        private Translation _translation;

        [SerializeField]
        private SystemLanguage _editorModeLanguage;
        private SystemLanguage? ActiveEditorModeLanguage { get; set; }

        private SystemLanguage? StoredLanguage { get; set; }
        public SystemLanguage Language => GetApplicationLanguage();

        public IEnumerable<SystemLanguage> AvailableLanguages =>
            _translation != null ? _translation.Languages : Array.Empty<SystemLanguage>();

        private bool IsStoredLanguageRead { get; set; }

        private void Update()
        {
            if (Application.isPlaying)
                return;

            if (_translation != null && _translation.RequiresRefresh())
            {
                _translation.RefreshLanguageData();
                RefreshLocalization();
            }

            if (_editorModeLanguage != ActiveEditorModeLanguage)
            {
                ActiveEditorModeLanguage = _editorModeLanguage;
                RefreshLocalization();
            }
        }

        public string GetLocalization(string key, string fallback = "n/a")
        {
            try
            {
                return _translation[(key, Language)] ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }

        public string GetClosestKey(string query, string fallback = null)
        {
            try
            {
                return _translation.GetClosestKey(query) ?? fallback;
            }
            catch
            {
                return fallback;
            }
        }

        public bool IsLocalizationAvailable(string key) => GetLocalization(key, null) != null;

        public void SetLanguage(SystemLanguage language)
        {
            PlayerPrefs.SetInt(KEY_STORED_LANGUAGE, (int)language);
            PlayerPrefs.Save();

            StoredLanguage = language;
            RefreshLocalization();
        }

        public void ClearLanguage()
        {
            PlayerPrefs.DeleteKey(KEY_STORED_LANGUAGE);
            PlayerPrefs.Save();

            StoredLanguage = null;
            RefreshLocalization();
        }

        private SystemLanguage? ReadStoredLanguage()
        {
            IsStoredLanguageRead = true;
            var value = PlayerPrefs.GetInt(KEY_STORED_LANGUAGE, -1);
            return Enum.IsDefined(typeof(SystemLanguage), value) ? (SystemLanguage)value : null;
        }

        private SystemLanguage GetApplicationLanguage()
        {
            if (Application.isPlaying.Not())
                return _editorModeLanguage;

            if (IsStoredLanguageRead.Not())
                StoredLanguage = ReadStoredLanguage();

            return StoredLanguage ?? Application.systemLanguage;
        }

        private void RefreshLocalization()
        {
            foreach (var item in FindObjectsOfType<MonoBehaviour>().OfType<ILocalizable>())
                item.Localize();
        }
    }
}
