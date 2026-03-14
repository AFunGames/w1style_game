using System;

namespace W1Style.Core.Interfaces
{
    /// <summary>
    /// Lightweight event bus for decoupled communication between systems.
    /// Prefer this over direct references when publisher and subscriber
    /// should not know about each other.
    /// </summary>
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler);
        void Unsubscribe<T>(Action<T> handler);
        void Publish<T>(T eventData);
        void Clear();
    }
}
