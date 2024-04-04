using System;

namespace MoonriseGames.Toolbox.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class NoValidationAttribute : Attribute { }
}
