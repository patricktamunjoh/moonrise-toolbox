using System.Linq;
using MoonriseGames.Toolbox.Constants;
using MoonriseGames.Toolbox.Validation;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [CreateAssetMenu(fileName = "Translation Sprites", menuName = "Toolbox/Localization/Translation Sprites", order = Orders.CREATE + 1)]
    public class TranslationSprites : ScriptableObject, IValidateable
    {
        [SerializeField]
        private Sprite _fallbackSprite;

        [SerializeField, Space]
        private TranslationValue<Sprite>[] _translations;

        public Sprite GetSprite(SystemLanguage language)
        {
            var sprite = _translations?.FirstOrDefault(x => x.Language == language)?.Value;
            return sprite ? sprite : _fallbackSprite;
        }

        public void Validate()
        {
            if (_translations.GroupBy(x => x.Language).Any(x => x.Count() > 1))
                throw new ValidationException("Multiple sprites for the same language");
        }
    }
}
