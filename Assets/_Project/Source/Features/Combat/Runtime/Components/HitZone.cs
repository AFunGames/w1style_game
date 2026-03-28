using UnityEngine;

namespace W1Style.Features.Combat.Components
{
    /// <summary>
    /// Marks a collider as a specific body zone (head or body).
    /// Used by the combat system to determine headshot damage.
    /// Attach to child colliders on an enemy.
    /// </summary>
    public sealed class HitZone : MonoBehaviour
    {
        [SerializeField] private ZoneType _zone = ZoneType.Body;

        public bool IsHead => _zone == ZoneType.Head;
        public ZoneType Zone => _zone;

        public enum ZoneType
        {
            Body,
            Head
        }
    }
}
