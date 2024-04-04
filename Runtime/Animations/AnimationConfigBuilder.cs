namespace MoonriseGames.Toolbox.Animations
{
    public class AnimationConfigBuilder
    {
        private bool UseUnscaledTime { get; set; } = false;

        private AnimationPlayback Playback { get; set; } = AnimationPlayback.Forward;
        private AnimationInterpolation Interpolation { get; set; } = AnimationInterpolation.Linear;

        public AnimationConfig Build() => new(UseUnscaledTime, Playback, Interpolation);

        public AnimationConfigBuilder WithUnscaledTime()
        {
            UseUnscaledTime = true;
            return this;
        }

        public AnimationConfigBuilder WithPlayback(AnimationPlayback playback)
        {
            Playback = playback;
            return this;
        }

        public AnimationConfigBuilder WithInterpolation(AnimationInterpolation interpolation)
        {
            Interpolation = interpolation;
            return this;
        }
    }
}
