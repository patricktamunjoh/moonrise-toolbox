using System.Collections;
using UnityEngine;

namespace MoonriseGames.Toolbox.Working
{
    public class WorkOnce : Work
    {
        private float Duration { get; }

        internal WorkOnce(MonoBehaviour context, float duration, bool useUnscaledTime = false)
            : base(context, useUnscaledTime) => Duration = duration;

        protected override IEnumerator GetExecutionCoroutine()
        {
            var timer = 0f;

            while (timer < Duration)
            {
                if (!IsPaused)
                {
                    OnExecutionProgress(Mathf.Clamp01(timer / Duration));
                    timer += UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                }

                yield return null;
            }

            OnExecutionProgress(1);
            OnExecutionCompleted();
        }
    }
}
