using UnityEngine;
using W1Style.Features.Combat.Components;

namespace W1Style.Features.Combat.Hazards
{
    /// <summary>
    /// Base hazard trigger zone. Enemies entering this trigger die instantly.
    /// Subclass for specific hazard visuals/behavior (spikes, fire, pit).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HazardZone : MonoBehaviour
    {
        [Header("Hazard")]
        [SerializeField] private float _damageAmount = 9999f;
        [SerializeField] private bool _instantKill = true;

        protected virtual void OnTriggerEnter(Collider other)
        {
            var damageable = other.GetComponentInParent<Damageable>();
            if (damageable == null) return;

            if (_instantKill)
                damageable.InstantKill();
        }
    }
}
