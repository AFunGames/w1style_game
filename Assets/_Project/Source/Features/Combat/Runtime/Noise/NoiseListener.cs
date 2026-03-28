using System;
using UnityEngine;

namespace W1Style.Features.Combat.Noise
{
    /// <summary>
    /// Listens for noise events emitted by <see cref="NoiseEmitter"/>.
    /// Attach to enemies so they can react to nearby noise.
    /// </summary>
    public sealed class NoiseListener : MonoBehaviour
    {
        /// <summary>
        /// Raised when a noise is heard within range.
        /// Parameter is the noise source position.
        /// </summary>
        public event Action<Vector3> OnNoiseHeard;

        private void OnEnable()
        {
            NoiseEmitter.Register(this);
        }

        private void OnDisable()
        {
            NoiseEmitter.Unregister(this);
        }

        /// <summary>
        /// Called by <see cref="NoiseEmitter"/> when a noise is within range.
        /// </summary>
        public void HearNoise(Vector3 noisePosition)
        {
            OnNoiseHeard?.Invoke(noisePosition);
        }
    }
}
