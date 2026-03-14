using UnityEngine;
using UnityEngine.InputSystem;
using W1Style.Features.Combat.Noise;
using W1Style.Features.Combat.Physics;

namespace W1Style.Features.Combat.Player
{
    /// <summary>
    /// Handles picking up and throwing physics objects.
    /// Picked-up objects are held in front of the camera.
    /// </summary>
    public sealed class PlayerThrow : MonoBehaviour
    {
        [Header("Pickup")]
        [SerializeField] private float _pickupRange = 3f;
        [SerializeField] private LayerMask _pickupLayers = ~0;
        [SerializeField] private Transform _holdPoint;

        [Header("Throw")]
        [SerializeField] private float _throwForce = 20f;
        [SerializeField] private float _throwNoiseRadius = 10f;

        [Header("References")]
        [SerializeField] private Transform _cameraTransform;

        private ThrowableObject _heldObject;

        private void Start()
        {
            if (_cameraTransform == null)
                _cameraTransform = GetComponentInChildren<Camera>().transform;

            if (_holdPoint == null)
            {
                var go = new GameObject("HoldPoint");
                go.transform.SetParent(_cameraTransform);
                go.transform.localPosition = new Vector3(0f, 0f, 1.5f);
                _holdPoint = go.transform;
            }
        }

        public void OnThrow(InputValue value)
        {
            if (!value.isPressed) return;

            if (_heldObject != null)
            {
                ThrowHeldObject();
                return;
            }

            TryPickup();
        }

        /// <summary>
        /// Force-pickup a specific throwable object. Called by <see cref="Interaction.PickupObject"/>.
        /// </summary>
        public bool TryPickupObject(ThrowableObject throwable)
        {
            if (_heldObject != null || throwable == null) return false;
            _heldObject = throwable;
            _heldObject.Pickup(_holdPoint);
            return true;
        }

        private void TryPickup()
        {
            Vector3 origin = _cameraTransform.position;
            Vector3 direction = _cameraTransform.forward;

            if (!UnityEngine.Physics.Raycast(origin, direction, out RaycastHit hit, _pickupRange, _pickupLayers))
                return;

            var throwable = hit.collider.GetComponentInParent<ThrowableObject>();
            if (throwable == null) return;

            _heldObject = throwable;
            _heldObject.Pickup(_holdPoint);
        }

        private void ThrowHeldObject()
        {
            Vector3 direction = _cameraTransform.forward;
            _heldObject.Throw(direction * _throwForce);
            NoiseEmitter.EmitNoise(_heldObject.transform.position, _throwNoiseRadius);
            _heldObject = null;
        }

        private void Update()
        {
            if (_heldObject == null) return;

            // Keep held object at hold point
            _heldObject.transform.position = Vector3.Lerp(
                _heldObject.transform.position, _holdPoint.position, Time.deltaTime * 15f);
        }
    }
}
