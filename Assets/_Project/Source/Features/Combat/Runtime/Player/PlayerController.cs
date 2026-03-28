using UnityEngine;
using UnityEngine.InputSystem;
using W1Style.Features.Combat.Noise;

namespace W1Style.Features.Combat.Player
{
    /// <summary>
    /// Rigidbody-based first-person player controller.
    /// Handles WASD movement, sprinting, jumping, and crouching.
    /// Requires a Rigidbody and CapsuleCollider on the same GameObject.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _crouchSpeed = 2.5f;

        [Header("Jump")]
        [SerializeField] private float _jumpForce = 6f;
        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayer = ~0;

        [Header("Crouch")]
        [SerializeField] private float _standHeight = 2f;
        [SerializeField] private float _crouchHeight = 1.2f;

        [Header("Sprint Noise")]
        [SerializeField] private float _sprintNoiseRadius = 12f;
        [SerializeField] private float _sprintNoiseInterval = 0.4f;

        private Rigidbody _rb;
        private CapsuleCollider _capsule;

        private Vector2 _moveInput;
        private bool _isSprinting;
        private bool _isCrouching;
        private bool _jumpRequested;
        private float _sprintNoiseTimer;

        public bool IsGrounded { get; private set; }
        public bool IsSprinting => _isSprinting && _moveInput.sqrMagnitude > 0.01f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _capsule = GetComponent<CapsuleCollider>();

            _rb.freezeRotation = true;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        public void OnMove(InputValue value) => _moveInput = value.Get<Vector2>();
        public void OnSprint(InputValue value) => _isSprinting = value.isPressed;
        public void OnJump(InputValue value) { if (value.isPressed) _jumpRequested = true; }

        public void OnCrouch(InputValue value)
        {
            _isCrouching = value.isPressed;
            _capsule.height = _isCrouching ? _crouchHeight : _standHeight;
            _capsule.center = new Vector3(0f, _capsule.height * 0.5f, 0f);
        }

        private void FixedUpdate()
        {
            CheckGround();
            Move();
            Jump();
            EmitSprintNoise();
        }

        private void CheckGround()
        {
            float radius = _capsule.radius * 0.9f;
            Vector3 origin = transform.position + Vector3.up * (radius + 0.01f);
            IsGrounded = UnityEngine.Physics.SphereCast(
                origin, radius, Vector3.down,
                out _, _groundCheckDistance + 0.01f, _groundLayer);
        }

        private void Move()
        {
            float speed = _isCrouching ? _crouchSpeed
                        : _isSprinting ? _sprintSpeed
                        : _moveSpeed;

            Vector3 forward = transform.forward;
            Vector3 right = transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 desiredVelocity = (forward * _moveInput.y + right * _moveInput.x) * speed;
            Vector3 velocity = _rb.linearVelocity;
            velocity.x = desiredVelocity.x;
            velocity.z = desiredVelocity.z;
            _rb.linearVelocity = velocity;
        }

        private void Jump()
        {
            if (!_jumpRequested || !IsGrounded) { _jumpRequested = false; return; }
            Vector3 velocity = _rb.linearVelocity;
            velocity.y = _jumpForce;
            _rb.linearVelocity = velocity;
            _jumpRequested = false;
        }

        private void EmitSprintNoise()
        {
            if (!IsSprinting) return;
            _sprintNoiseTimer -= Time.fixedDeltaTime;
            if (_sprintNoiseTimer > 0f) return;
            _sprintNoiseTimer = _sprintNoiseInterval;
            NoiseEmitter.EmitNoise(transform.position, _sprintNoiseRadius);
        }
    }
}
