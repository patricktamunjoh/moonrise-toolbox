using MoonriseGames.Toolbox.Extensions;
using MoonriseGames.Toolbox.Textures;
using UnityEngine;

namespace MoonriseGames.Toolbox.Constants
{
    public static class Sprites
    {
        private static Sprite _errorSprite;

        public static Sprite ERROR => _errorSprite ??= TextureFactory.GetCheckerboardTexture(10, Color.yellow, Color.black).ToSprite();
    }
}
