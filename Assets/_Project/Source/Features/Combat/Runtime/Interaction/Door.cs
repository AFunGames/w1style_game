using UnityEngine;
using W1Style.Features.Combat.Domain;

namespace W1Style.Features.Combat.Interaction
{
    /// <summary>
    /// Simple door that can be opened/closed via interaction.
    /// Rotates around its pivot when interacted with.
    /// </summary>
    public sealed class Door : MonoBehaviour, IInteractable
    {
        [Header("Door Settings")]
        [SerializeField] private float _openAngle = 90f;
        [SerializeField] private float _openSpeed = 3f;

        private bool _isOpen;
        private Quaternion _closedRotation;
        private Quaternion _openRotation;

        public string InteractionPrompt => _isOpen ? "Close Door" : "Open Door";
        public bool CanInteract => true;

        private void Awake()
        {
            _closedRotation = transform.localRotation;
            _openRotation = _closedRotation * Quaternion.Euler(0f, _openAngle, 0f);
        }

        public void Interact(GameObject instigator)
        {
            _isOpen = !_isOpen;
        }

        private void Update()
        {
            Quaternion target = _isOpen ? _openRotation : _closedRotation;
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation, target, Time.deltaTime * _openSpeed);
        }
    }
}
