using System;
using System.Collections.Generic;
using System.Linq;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [Serializable]
    internal class TranslationMap<T>
    {
        private char[] QuerySeparators { get; } = new[] { ' ', '-', '_', ':' };

        [SerializeField]
        private TranslationSet<T>[] _sets;

        [SerializeField]
        private SystemLanguage[] _languages;
        public SystemLanguage[] Languages => _languages;

        private HashSet<SystemLanguage> _languagesSet;
        private HashSet<SystemLanguage> LanguagesSet => _languagesSet ??= _languages.ToHashSet();

        private Dictionary<string, TranslationSet<T>> _setsDict;
        private Dictionary<string, TranslationSet<T>> SetsDict => _setsDict ??= _sets.ToDictionary(x => x.Key, x => x);

        public int Count => _sets.Length;

        public TranslationMap(TranslationSet<T>[] sets)
        {
            if (sets == null || sets.Length == 0)
                throw new ArgumentException("Set array is null or empty");

            _languages = sets[0].Languages;

            if (sets.Any(x => x.Languages.SequenceEqual(_languages).Not()))
                throw new ArgumentException("Set array contains elements with different languages");

            _sets = sets;
        }

        public T this[(string key, SystemLanguage language) index] => SetsDict[index.key.ToLower()][index.language];

        public bool ContainsKey(string key) => SetsDict.ContainsKey(key.ToLower());

        public bool ContainsLanguage(SystemLanguage language) => LanguagesSet.Contains(language);

        public string GetClosestKey(string query)
        {
            if (string.IsNullOrEmpty(query))
                return null;

            var suggestion = null as string;
            var queryElements = query.ToLower().Split(QuerySeparators);

            foreach (var i in _sets)
            {
                if (!string.IsNullOrEmpty(suggestion) && i.Key.Length >= suggestion.Length)
                    continue;

                if (queryElements.Any(x => i.Key.Contains(x).Not()))
                    continue;

                suggestion = i.Key;
            }

            return suggestion;
        }
    }
}
