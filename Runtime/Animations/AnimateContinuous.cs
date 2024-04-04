using System;
using MoonriseGames.Toolbox.Working;
using UnityEngine;

namespace MoonriseGames.Toolbox.Animations
{
    public class AnimateContinuous : Animate
    {
        private Action<float> ApplyProgress { get; }
        private Action ApplyInitialState { get; }

        public override event Action OnComplete;

        public AnimateContinuous(MonoBehaviour context, AnimationConfig config, Action onReset, Action<float> onProgress)
            : base(config)
        {
            ApplyProgress = onProgress;
            ApplyInitialState = onReset;

            Work = new WorkAlways(context, config.UseUnscaledTime);
            Work.OnStart += RestoreInitialState;
            Work.OnProgress += OnWorkProgress;
        }

        protected override void RestoreInitialState() => ApplyInitialState?.Invoke();

        private void OnWorkProgress(float progress)
        {
            progress = IsReversed ? -progress : progress;
            ApplyProgress?.Invoke(progress);
        }
    }
}
