using MoonriseGames.Toolbox.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateGraphicBuilder : AnimateBuilder<AnimateGraphicBuilder>
    {
        private Graphic Graphic { get; }

        public AnimateGraphicBuilder(Graphic graphic, MonoBehaviour context)
            : base(context) => Graphic = graphic;

        public AnimateGraphicBuilder(Graphic graphic)
            : base(graphic) => Graphic = graphic;

        public override AnimateGraphicBuilder Delay(float duration)
        {
            AddDelay(duration);
            return this;
        }

        public AnimateGraphicBuilder FadeTo(float duration, AnimationConfig config, float to) =>
            Fade(duration, config, Graphic.color.a, to);

        public AnimateGraphicBuilder Fade(float duration, AnimationConfig config, float from, float to)
        {
            void OnProgress(float progress)
            {
                if (Graphic.IsNull())
                    return;

                Graphic.color = Graphic.color.WithAlpha(Mathf.Lerp(from, to, progress));
            }

            AddAnimation(new AnimateBetween(Context, duration, config, OnProgress));
            return this;
        }
    }
}
