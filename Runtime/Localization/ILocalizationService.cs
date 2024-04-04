using System.Collections.Generic;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    public interface ILocalizationService
    {
        SystemLanguage Language { get; }

        IEnumerable<SystemLanguage> AvailableLanguages { get; }

        string GetLocalization(string key, string fallback = "n/a");

        string GetClosestKey(string query, string fallback = null);

        bool IsLocalizationAvailable(string key);

        void SetLanguage(SystemLanguage language);

        void ClearLanguage();
    }
}
