using UnityEngine;

namespace MoonriseGames.Toolbox.Tests.Utilities
{
    public static class AudioFactory
    {
        public static AudioClip GetAudioClip(int length = 1) => AudioClip.Create("example clip", 44100 * length, 1, 44100, false);
    }
}
