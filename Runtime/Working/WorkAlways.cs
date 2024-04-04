using System.Collections;
using UnityEngine;

namespace MoonriseGames.Toolbox.Working
{
    public class WorkAlways : Work
    {
        internal WorkAlways(MonoBehaviour context, bool useUnscaledTime = false)
            : base(context, useUnscaledTime) { }

        protected override IEnumerator GetExecutionCoroutine()
        {
            while (true)
            {
                if (!IsPaused)
                    OnExecutionProgress(UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

                yield return null;
            }
        }
    }
}
