using System.Collections.Generic;
using UnityEngine;

namespace W1Style.Features.Combat.Noise
{
    /// <summary>
    /// Static noise emission system.
    /// When noise is emitted at a position, all <see cref="NoiseListener"/>
    /// instances within range are notified.
    /// </summary>
    public static class NoiseEmitter
    {
        private static readonly List<NoiseListener> _listeners = new();

        /// <summary>
        /// Register a listener. Called automatically by <see cref="NoiseListener"/>.
        /// </summary>
        public static void Register(NoiseListener listener)
        {
            if (!_listeners.Contains(listener))
                _listeners.Add(listener);
        }

        /// <summary>
        /// Unregister a listener. Called automatically by <see cref="NoiseListener"/>.
        /// </summary>
        public static void Unregister(NoiseListener listener)
        {
            _listeners.Remove(listener);
        }

        /// <summary>
        /// Emits a noise event at the given position.
        /// All listeners within the radius are notified.
        /// </summary>
        public static void EmitNoise(Vector3 position, float radius)
        {
            float sqrRadius = radius * radius;
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                if (i >= _listeners.Count) continue;
                var listener = _listeners[i];
                if (listener == null) continue;

                float sqrDist = (listener.transform.position - position).sqrMagnitude;
                if (sqrDist <= sqrRadius)
                    listener.HearNoise(position);
            }
        }
    }
}
