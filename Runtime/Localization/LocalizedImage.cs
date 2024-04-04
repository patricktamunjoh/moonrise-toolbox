using MoonriseGames.Toolbox.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace MoonriseGames.Toolbox.Localization
{
    [ExecuteInEditMode]
    internal class LocalizedImage : MonoBehaviour, ILocalizable
    {
        [SerializeField]
        private TranslationSprites _translations;

        private Image Image { get; set; }
        private SpriteRenderer SpriteRenderer { get; set; }

        private void Awake()
        {
            Image = GetComponent<Image>();
            SpriteRenderer = GetComponent<SpriteRenderer>();

            Localize();
        }

        private void Update()
        {
            if (!Application.isPlaying)
                Localize();
        }

        public void Localize()
        {
            try
            {
                if (Image != null)
                    Image.sprite = GetSprite();

                if (SpriteRenderer != null)
                    SpriteRenderer.sprite = GetSprite();
            }
            catch (SingletonNotAvailableException e)
            {
                if (Application.isPlaying)
                    throw e;
            }
        }

        private Sprite GetSprite()
        {
            if (_translations == null)
                return null;

            return _translations.GetSprite(LocalizationService.Unit.Language);
        }
    }
}
