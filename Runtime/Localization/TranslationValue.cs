using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [Serializable]
    internal class TranslationValue<T>
    {
        [SerializeField]
        private SystemLanguage _language;
        public SystemLanguage Language => _language;

        [SerializeField]
        private T _value;
        public T Value => _value;

        public TranslationValue(SystemLanguage language, T value)
        {
            _language = language;
            _value = value;
        }
    }
}
