using UnityEngine;

namespace W1Style.Features.Combat.Domain
{
    /// <summary>
    /// Generic interface for objects the player can interact with (E key).
    /// Implement on doors, levers, pickups, etc.
    /// </summary>
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        bool CanInteract { get; }
        void Interact(GameObject instigator);
    }
}
