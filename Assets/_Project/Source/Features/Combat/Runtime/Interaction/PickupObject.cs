using UnityEngine;
using W1Style.Features.Combat.Domain;
using W1Style.Features.Combat.Physics;
using W1Style.Features.Combat.Player;

namespace W1Style.Features.Combat.Interaction
{
    /// <summary>
    /// Pickup object that implements <see cref="IInteractable"/>.
    /// When interacted with via E key, it is picked up by the player's throw system.
    /// </summary>
    [RequireComponent(typeof(ThrowableObject))]
    public sealed class PickupObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _itemName = "Object";

        private ThrowableObject _throwable;
        private Rigidbody _rigidbody;

        public string InteractionPrompt => $"Pick up {_itemName}";
        public bool CanInteract => !_rigidbody.isKinematic;

        private void Awake()
        {
            _throwable = GetComponent<ThrowableObject>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Interact(GameObject instigator)
        {
            var playerThrow = instigator.GetComponent<PlayerThrow>();
            if (playerThrow == null)
                playerThrow = instigator.GetComponentInChildren<PlayerThrow>();

            if (playerThrow != null)
                playerThrow.TryPickupObject(_throwable);
        }
    }
}
