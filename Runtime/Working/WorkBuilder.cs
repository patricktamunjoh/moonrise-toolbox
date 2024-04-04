using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Working
{
    public class WorkBuilder
    {
        private IWork Work { get; set; }

        public WorkBuilder(MonoBehaviour context, float duration, bool useUnscaledTime = false) =>
            Work = new WorkOnce(context, duration, useUnscaledTime);

        public WorkBuilder(MonoBehaviour context, bool useUnscaledTime = false) => Work = new WorkAlways(context, useUnscaledTime);

        public IWork Build()
        {
            var work = Work;
            Work = null;
            return work;
        }

        public WorkBuilder OnProgress(Action<float> action)
        {
            Work.OnProgress += action;
            return this;
        }

        public WorkBuilder OnStart(Action action)
        {
            Work.OnStart += action;
            return this;
        }

        public WorkBuilder OnStop(Action action)
        {
            Work.OnStop += action;
            return this;
        }

        public WorkBuilder OnPause(Action action)
        {
            Work.OnPause += action;
            return this;
        }

        public WorkBuilder OnResume(Action action)
        {
            Work.OnResume += action;
            return this;
        }

        public WorkBuilder OnComplete(Action action)
        {
            Work.OnComplete += action;
            return this;
        }
    }
}
