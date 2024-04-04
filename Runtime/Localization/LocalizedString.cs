using System;
using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Validation;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [Serializable]
    public struct LocalizedString : IValidateable
    {
        [SerializeField]
        private string _key;

        public string Value => LocalizationService.Unit.GetLocalization(_key);

        public LocalizedString(string key) => _key = key;

        public override string ToString() => Value;

        public static implicit operator string(LocalizedString localizedString) => localizedString.ToString();

        public void Validate()
        {
            if (string.IsNullOrEmpty(_key.Trim()))
                throw new ValidationException("Translation key is null or empty");

            try
            {
                if (LocalizationService.Unit.IsLocalizationAvailable(_key).Not())
                    throw new ValidationException($"Missing translation for key \"{_key}\"");
            }
            catch (SingletonNotAvailableException e)
            {
                throw new ValidationException(e.Message);
            }
        }
    }
}
