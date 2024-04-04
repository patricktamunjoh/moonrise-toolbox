namespace MoonriseGames.Toolbox.Animations
{
    public class AnimationConfig
    {
        public bool UseUnscaledTime { get; }

        public AnimationPlayback Playback { get; }
        public AnimationInterpolation Interpolation { get; }

        internal AnimationConfig(bool useUnscaledTime, AnimationPlayback playback, AnimationInterpolation interpolation)
        {
            UseUnscaledTime = useUnscaledTime;
            Playback = playback;
            Interpolation = interpolation;
        }
    }
}
