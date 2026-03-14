using UnityEngine;
using UnityEngine.InputSystem;
using W1Style.Features.Combat.Domain;

namespace W1Style.Features.Combat.Player
{
    /// <summary>
    /// Handles player interaction with <see cref="IInteractable"/> objects.
    /// Uses a forward raycast from the camera to detect interactables.
    /// </summary>
    public sealed class PlayerInteraction : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _interactRange = 3f;
        [SerializeField] private LayerMask _interactLayers = ~0;

        [Header("References")]
        [SerializeField] private Transform _cameraTransform;

        private void Start()
        {
            if (_cameraTransform == null)
                _cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        public void OnInteract(InputValue value)
        {
            if (!value.isPressed) return;

            Vector3 origin = _cameraTransform.position;
            Vector3 direction = _cameraTransform.forward;

            if (!UnityEngine.Physics.Raycast(origin, direction, out RaycastHit hit, _interactRange, _interactLayers))
                return;

            var interactable = hit.collider.GetComponentInParent<IInteractable>() as Component;
            if (interactable == null)
                interactable = hit.collider.GetComponent<Component>();

            if (interactable is IInteractable target && target.CanInteract)
                target.Interact(gameObject);
        }
    }
}
