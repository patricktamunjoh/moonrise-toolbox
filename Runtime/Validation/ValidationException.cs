using System;

namespace MoonriseGames.Toolbox.Validation
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : base(message) { }
    }
}
