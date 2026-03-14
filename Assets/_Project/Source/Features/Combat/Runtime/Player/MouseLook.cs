using UnityEngine;
using UnityEngine.InputSystem;

namespace W1Style.Features.Combat.Player
{
    /// <summary>
    /// First-person mouse look controller.
    /// Attach to the player root; controls camera pitch via child camera transform.
    /// </summary>
    public sealed class MouseLook : MonoBehaviour
    {
        [Header("Sensitivity")]
        [SerializeField] private float _sensitivity = 2f;

        [Header("Pitch Clamp")]
        [SerializeField] private float _minPitch = -80f;
        [SerializeField] private float _maxPitch = 80f;

        [Header("References")]
        [SerializeField] private Transform _cameraTransform;

        private float _pitch;
        private Vector2 _lookInput;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (_cameraTransform == null)
                _cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        /// <summary>
        /// Called by PlayerInput component via Unity Input System.
        /// </summary>
        public void OnLook(InputValue value)
        {
            _lookInput = value.Get<Vector2>();
        }

        private void LateUpdate()
        {
            float yaw = _lookInput.x * _sensitivity;
            _pitch -= _lookInput.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

            transform.Rotate(0f, yaw, 0f);
            _cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
