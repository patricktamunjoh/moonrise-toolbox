using System;

namespace MoonriseGames.Toolbox.Architecture
{
    public class SingletonNotAvailableException : Exception
    {
        public SingletonNotAvailableException(string message)
            : base(message) { }
    }
}
