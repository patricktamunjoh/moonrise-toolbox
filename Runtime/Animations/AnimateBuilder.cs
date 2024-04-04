using System.Collections.Generic;
using UnityEngine;

namespace MoonriseGames.Toolbox.Animations
{
    public abstract class AnimateBuilder<T>
        where T : AnimateBuilder<T>
    {
        protected MonoBehaviour Context { get; }

        private List<IAnimateDirectional> Animations { get; } = new();
        private float LastDelay { get; set; }

        protected AnimateBuilder(MonoBehaviour context) => Context = context;

        public IAnimate Build()
        {
            var animation = null as IAnimateDirectional;

            if (Animations.Count == 1)
                animation = Animations[0];

            if (Animations.Count > 1)
                animation = new AnimateTogether(Animations.ToArray());

            Animations.Clear();
            return LastDelay > 0 ? new AnimateSequence(GetDelayAnimation(LastDelay), animation) : animation;
        }

        public IAnimate Build(AnimationPlayback sequencePlayback)
        {
            var animation = null as IAnimate;

            if (Animations.Count > 0)
                animation = new AnimateSequence(sequencePlayback, Animations.ToArray());

            Animations.Clear();
            return animation;
        }

        public abstract T Delay(float duration);

        protected void AddDelay(float duration)
        {
            LastDelay = Mathf.Max(0, duration);

            if (duration <= 0)
                return;

            Animations.Add(GetDelayAnimation(duration));
        }

        private AnimateBetween GetDelayAnimation(float duration)
        {
            var config = new AnimationConfigBuilder().WithPlayback(AnimationPlayback.Forward).Build();
            return new AnimateBetween(Context, duration, config, null);
        }

        protected void AddAnimation(IAnimateDirectional animation) => Animations.Add(animation);
    }
}
