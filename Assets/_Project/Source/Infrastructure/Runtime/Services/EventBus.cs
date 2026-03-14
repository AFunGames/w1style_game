using System;
using System.Collections.Generic;
using W1Style.Core.Interfaces;

namespace W1Style.Infrastructure.Services
{
    /// <summary>
    /// Simple type-keyed event bus for decoupled communication.
    /// Events are structs published and received by type.
    /// Bound as a global singleton via ProjectInstaller.
    /// </summary>
    public sealed class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _handlers[type] = list;
            }

            list.Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var list))
            {
                list.Remove(handler);
            }
        }

        public void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
                return;

            // Iterate over a copy to allow modifications during dispatch
            var snapshot = list.ToArray();
            foreach (var handler in snapshot)
            {
                ((Action<T>)handler).Invoke(eventData);
            }
        }

        public void Clear()
        {
            _handlers.Clear();
        }
    }
}
