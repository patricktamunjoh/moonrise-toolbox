using MoonriseGames.Toolbox.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace MoonriseGames.Toolbox.Extensions
{
    public static class AnimateExtensions
    {
        public static AnimateTransformBuilder Animate(this Transform transform, MonoBehaviour context) => new(transform, context);

        public static AnimateTransformBuilder Animate(this MonoBehaviour context) => new(context);

        public static AnimateGraphicBuilder Animate(this Graphic graphic, MonoBehaviour context) => new(graphic, context);

        public static AnimateGraphicBuilder Animate(this Graphic graphic) => new(graphic);
    }
}
