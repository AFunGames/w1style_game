using UnityEngine;
using W1Style.Features.Combat.Domain;
using W1Style.Features.Combat.Physics;

namespace W1Style.Features.Combat.Interaction
{
    /// <summary>
    /// Pickup object that implements <see cref="IInteractable"/>.
    /// When interacted with, it is picked up by the player's throw system.
    /// Also acts as a <see cref="ThrowableObject"/> wrapper for interaction.
    /// </summary>
    [RequireComponent(typeof(ThrowableObject))]
    public sealed class PickupObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _itemName = "Object";

        private ThrowableObject _throwable;

        public string InteractionPrompt => $"Pick up {_itemName}";
        public bool CanInteract => !_throwable.gameObject.GetComponent<Rigidbody>().isKinematic;

        private void Awake()
        {
            _throwable = GetComponent<ThrowableObject>();
        }

        public void Interact(GameObject instigator)
        {
            // Pickup is handled by PlayerThrow via the Q key.
            // This interaction serves as a secondary pickup method via E key.
            var playerThrow = instigator.GetComponent<Player.PlayerThrow>();
            if (playerThrow == null)
                playerThrow = instigator.GetComponentInChildren<Player.PlayerThrow>();
            // Pickup handled through PlayerThrow's Q input; this provides the prompt only.
        }
    }
}
