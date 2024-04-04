using UnityEngine;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class TextureExtensions
    {
        public static Sprite ToSprite(this Texture2D texture) =>
            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2, texture.width);
    }
}
