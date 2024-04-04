using System;
using System.Collections.Generic;
using MoonriseGames.Toolbox.Architecture;
using MoonriseGames.Toolbox.Extensions;
using UnityEngine;

namespace MoonriseGames.Toolbox.Messaging
{
    public class MessagingService : SingletonGlobal<IMessagingService>, IMessagingService
    {
        private List<(MonoBehaviour subscriber, Type type, Action<object> callback)> Subscriptions { get; } = new();

        public void Subscribe(MonoBehaviour subscriber, Action<Message> callback) =>
            Subscriptions.Add((subscriber, null, x => callback?.Invoke(x as Message)));

        public void Subscribe<T>(MonoBehaviour subscriber, Action callback)
            where T : Message => Subscriptions.Add((subscriber, typeof(T), x => callback?.Invoke()));

        public void Subscribe<T>(MonoBehaviour subscriber, Action<T> callback)
            where T : Message => Subscriptions.Add((subscriber, typeof(T), x => callback?.Invoke(x as T)));

        public void Send(Message message)
        {
            for (var i = 0; i < Subscriptions.Count; i++)
            {
                var subscription = Subscriptions[i];

                if (subscription.subscriber.IsNull())
                {
                    Subscriptions.RemoveAt(i--);
                    continue;
                }

                if (subscription.type == null || subscription.type.IsInstanceOfType(message))
                    subscription.callback.Invoke(message);
            }
        }

        public void Flush() => Send(null);
    }
}
