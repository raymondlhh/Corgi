using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CorgiMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)] private float walkSpeed = 2.8f;
    [SerializeField, Min(0f)] private float runSpeed = 5.2f;
    [SerializeField, Min(0f)] private float acceleration = 12f;
    [SerializeField, Range(0.1f, 20f)] private float rotationSpeed = 12f;

    [Header("Jumping & Gravity")]
    [SerializeField, Min(0f)] private float jumpHeight = 0.8f;
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float terminalVelocity = -40f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController _controller;
    private Vector3 _velocity;
    private float _currentSpeed;

    public bool IsRunning { get; private set; }
    public bool IsGrounded => _controller != null && _controller.isGrounded;
    public float CurrentSpeed => _currentSpeed;
    public Vector3 CurrentVelocity => _velocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (_controller == null) return;

        HandleMovement(Time.deltaTime);
    }

    private void HandleMovement(float deltaTime)
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool hasInput = moveInput.sqrMagnitude > 0.001f;
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        IsRunning = Input.GetKey(KeyCode.LeftShift) && hasInput;
        float targetSpeed = (IsRunning ? runSpeed : walkSpeed) * moveInput.magnitude;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, acceleration * deltaTime);

        Vector3 moveDirection = CalculateMoveDirection(moveInput);

        if (hasInput && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * deltaTime);
        }

        Vector3 horizontalVelocity = moveDirection * _currentSpeed;
        _velocity.x = horizontalVelocity.x;
        _velocity.z = horizontalVelocity.z;

        if (IsGrounded && _velocity.y < 0f)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y = Mathf.Max(_velocity.y + gravity * deltaTime, terminalVelocity);

        _controller.Move(_velocity * deltaTime);
    }

    private Vector3 CalculateMoveDirection(Vector2 moveInput)
    {
        if (cameraTransform == null)
        {
            return new Vector3(moveInput.x, 0f, moveInput.y);
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Align input with camera so forward always means "away from the camera".
        return (forward * moveInput.y + right * moveInput.x).normalized;
    }
}

