using System;
using UnityEngine;

namespace MoonriseGames.Toolbox.Messaging
{
    public interface IMessagingService
    {
        void Subscribe(MonoBehaviour subscriber, Action<Message> callback);

        void Subscribe<T>(MonoBehaviour subscriber, Action callback)
            where T : Message;

        void Subscribe<T>(MonoBehaviour subscriber, Action<T> callback)
            where T : Message;

        void Send(Message message);

        void Flush();
    }
}
