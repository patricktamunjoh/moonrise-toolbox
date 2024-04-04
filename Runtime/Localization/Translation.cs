using System;
using System.Linq;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Validation;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [CreateAssetMenu(fileName = "Translation", menuName = "Toolbox/Localization/Translation", order = Orders.CREATE)]
    public class Translation : ScriptableObject, IValidateable
    {
        [SerializeField, HideInInspector]
        private char _delimiter = ',';

        [SerializeField, HideInInspector]
        private char _escape = '"';

        [SerializeField, HideInInspector]
        private char _comment = '#';

        [SerializeField]
        private SystemLanguage _fallbackLanguage = SystemLanguage.English;

        [SerializeField, HideInInspector]
        private TextAsset[] _languageFiles;
        public SystemLanguage[] Languages => _translationMap?.Languages ?? Array.Empty<SystemLanguage>();

        [SerializeField, HideInInspector]
        private TranslationMap<string> _translationMap;

        [SerializeField, HideInInspector]
        private int _lastUpdateHash;

        public string this[(string key, SystemLanguage language) index]
        {
            get
            {
                if (string.IsNullOrEmpty(index.key) || _translationMap == null || _translationMap.ContainsKey(index.key).Not())
                    return null;

                if (_translationMap.ContainsLanguage(index.language).Not() && _translationMap.ContainsLanguage(_fallbackLanguage).Not())
                    return null;

                return _translationMap.ContainsLanguage(index.language)
                    ? _translationMap[index]
                    : _translationMap[(index.key, _fallbackLanguage)];
            }
        }

        public void RefreshLanguageData()
        {
#if UNITY_EDITOR

            _lastUpdateHash = GetLanguageFilesHashCode();
            _translationMap = null;

            var reader = new LanguageFileReader(_delimiter, _escape, _comment);
            _translationMap = reader.ReadTranslationMap(_languageFiles);
            UnityEditor.EditorUtility.SetDirty(this);

#endif
        }

        public bool RequiresRefresh() => _languageFiles != null && _lastUpdateHash != GetLanguageFilesHashCode();

        private int GetLanguageFilesHashCode() =>
            _languageFiles == null ? 0 : _languageFiles.Select(x => x.text).Aggregate(string.Empty, (a, b) => a + b).GetHashCode();

        public string GetClosestKey(string query) => _translationMap?.GetClosestKey(query);

        public void Validate()
        {
            if (_languageFiles == null || _languageFiles.Length == 0)
                throw new ValidationException("Missing language files");

            if (_translationMap != null && _translationMap.ContainsLanguage(_fallbackLanguage).Not())
                throw new ValidationException("Fallback language not contained in language files");
        }
    }
}
