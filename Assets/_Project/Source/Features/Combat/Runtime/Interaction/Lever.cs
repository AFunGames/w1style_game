using UnityEngine;
using W1Style.Features.Combat.Domain;

namespace W1Style.Features.Combat.Interaction
{
    /// <summary>
    /// Lever that can be toggled on/off via interaction.
    /// Raises an event (via UnityEvent) when activated.
    /// </summary>
    public sealed class Lever : MonoBehaviour, IInteractable
    {
        [Header("Lever Settings")]
        [SerializeField] private float _activatedAngle = -45f;
        [SerializeField] private float _deactivatedAngle = 45f;
        [SerializeField] private float _lerpSpeed = 5f;

        [Header("Events")]
        [SerializeField] private UnityEngine.Events.UnityEvent _onActivated;
        [SerializeField] private UnityEngine.Events.UnityEvent _onDeactivated;

        private bool _isActivated;

        public string InteractionPrompt => _isActivated ? "Deactivate Lever" : "Activate Lever";
        public bool CanInteract => true;

        public void Interact(GameObject instigator)
        {
            _isActivated = !_isActivated;

            if (_isActivated)
                _onActivated?.Invoke();
            else
                _onDeactivated?.Invoke();
        }

        private void Update()
        {
            float targetAngle = _isActivated ? _activatedAngle : _deactivatedAngle;
            Quaternion target = Quaternion.Euler(targetAngle, 0f, 0f);
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation, target, Time.deltaTime * _lerpSpeed);
        }
    }
}
