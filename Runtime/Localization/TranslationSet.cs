using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [Serializable]
    internal class TranslationSet<T>
    {
        [SerializeField]
        private string _key;
        public string Key => _key;

        [SerializeField]
        private TranslationValue<T>[] _values;

        private Dictionary<SystemLanguage, T> _valuesDict;
        private Dictionary<SystemLanguage, T> ValuesDict => _valuesDict ??= _values.ToDictionary(x => x.Language, x => x.Value);

        public SystemLanguage[] Languages => _values.Select(x => x.Language).ToArray();

        public TranslationSet(string key, TranslationValue<T>[] values)
        {
            if (values == null || values.Length == 0)
                throw new ArgumentException("Value array is null or empty");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Set key is null or empty");

            _key = key.ToLower();
            _values = values;
        }

        public T this[SystemLanguage language] => ValuesDict[language];
    }
}
