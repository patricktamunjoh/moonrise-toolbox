using MoonriseGames.Toolbox.Extensions;

namespace MoonriseGames.Toolbox.Messaging
{
    public abstract class Message
    {
        public virtual MessageType Type => MessageType.Instant;

        protected virtual string DescriptionKey => null;

        public string Description => DescriptionKey?.Localized();
    }
}
