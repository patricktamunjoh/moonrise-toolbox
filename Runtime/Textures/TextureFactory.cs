using UnityEngine;

namespace MoonriseGames.Toolbox.Textures
{
    public static class TextureFactory
    {
        public static Texture2D GetCheckerboardTexture(int size, Color color01, Color color02)
        {
            var texture = new Texture2D(size, size) { filterMode = FilterMode.Point };
            var pixels = new Color[size * size];

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                pixels[x + y * size] = (x + y) % 2 == 0 ? color01 : color02;

            texture.SetPixels(pixels);
            texture.Apply();

            return texture;
        }

        public static Texture2D GetOnePixelTexture(Color color)
        {
            var texture = new Texture2D(1, 1);

            texture.SetPixel(0, 0, color, 0);
            texture.Apply();

            return texture;
        }
    }
}
