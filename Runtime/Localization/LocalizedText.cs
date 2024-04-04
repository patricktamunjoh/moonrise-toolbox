using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Validation;
using TMPro;
using UnityEngine;

namespace MoonriseGames.Toolbox.Localization
{
    [NoValidation, ExecuteInEditMode]
    internal class LocalizedText : MonoBehaviour, ILocalizable
    {
        [SerializeField]
        private LocalizedString _text;

        private TextMeshProUGUI TextUi { get; set; }
        private TextMeshPro Text { get; set; }

        private void Awake()
        {
            TextUi = GetComponent<TextMeshProUGUI>();
            Text = GetComponent<TextMeshPro>();

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
                if (TextUi != null)
                    TextUi.text = _text;

                if (Text != null)
                    Text.text = _text;
            }
            catch (SingletonNotAvailableException e)
            {
                if (Application.isPlaying)
                    throw e;
            }
        }
    }
}
