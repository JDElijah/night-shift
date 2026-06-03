using UnityEngine;
using UnityEngine.InputSystem;

/*
    FpsMovement.cs handles first person playermovement using Unity's CharacterController and the New Input System.
    
    Responsibilities: 
        Walking
        Jumping and gravity
        Crouching and standing
        Smooth crouch height and camera transitions
 */

public class FpsMovement : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float crouchSpeed = 2f;

    [Header("Jump and Fall")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float gravity = -12f;                      // Gravity applied manually. CharacterController does not use unity Physics gravtiy automatically.
    [SerializeField] private float initialFallVelocity = -2f;           // Minimal downlard force to keep the player grounded when standing on slopes or uneven surface.

    [Header("Crouching")]
    [SerializeField] private float standingHeight = 2f;
    [SerializeField] private float crouchingHeight = 1f;
    [SerializeField] private float crouchTransitionSpeed = 10f;         // How quickly the CharacterController's height changes when crouching or standing. 
    [SerializeField] private float cameraOffset = 0.4f;                 // Camera set slightly below the top of the player capsule. 

    [Header("References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference crouchAction;
    [SerializeField] private InputActionReference sprintAction;

    private CharacterController _characterController;
    private Vector2 _moveInput;                                         // Store the current movement input from the player. 
    private bool _isGrounded;
    private bool _isRunning;
    private bool _isCrouching; 
    private float _verticalVelocity;                                    // Track upward and downward movement separately from horizontal movement. 
    private float _targetHeight;                                        // The height the CharacterController is currently moving toward. 
    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _targetHeight = standingHeight;                                  // Start the character at full standing height. 

    }

    private void OnEnable()
    {
        // Subscribe to input events when this script becomes active.
        moveAction.action.performed += StoreMovementInput;
        moveAction.action.canceled += StoreMovementInput;
        jumpAction.action.performed += Jump;
        sprintAction.action.performed += Sprint;
        sprintAction.action.canceled += Sprint; 
        crouchAction.action.performed += Crouch;
    }

    private void OnDisable()
    {
        // Unsubscribe from input events to prevent duplicate subscriptions or callbacks after this object is disabled. 
        moveAction.action.performed -= StoreMovementInput;
        moveAction.action.canceled -= StoreMovementInput;
        jumpAction.action.performed -= Jump;
        sprintAction.action.performed -= Sprint;
        sprintAction.action.canceled -= Sprint;
        crouchAction.action.performed -= Crouch; 
    }

    

    private void Update()
    {
        _isGrounded = _characterController.isGrounded;
        HandleGravity(); 
        HandleMovement();
        HandleCrouchTransition(); 
    }

    private void StoreMovementInput(InputAction.CallbackContext context)
    {
        // Reaed the current 2D movement input.
        // X = left/right, Y = forward/back.
        _moveInput = context.ReadValue<Vector2>(); 
    }

    private void Jump(InputAction.CallbackContext context)
    {
        // Only allow jumping while grounded. 
        if (_isGrounded)
        {
            _verticalVelocity = jumpForce;
        }
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (_isCrouching)
        {   
            // Before standing, check that there is enough space above the player.
            if (!CanStandUp())
            {
                return;
            }
            _targetHeight = standingHeight; 
        }
        else 
        {
            _targetHeight = crouchingHeight; 
        }
        
        // Toggle crouch state after choosing the new target height.
        _isCrouching = !_isCrouching;

    }

    private bool CanStandUp()
    {
        float radius = _characterController.radius;

        // Amount of vertical space needed to return to standing height.
        float castDistance = standingHeight - _characterController.height; 

        // Bottom and top points of the current character capsule.
        Vector3 bottom = transform.position + _characterController.center + Vector3.down * (_characterController.height / 2f - radius);
        Vector3 top = transform.position + _characterController.center + Vector3.up * (_characterController.height / 2f - radius);

        // Cast the current capsule upward to see wether standing would hit anything.
        return !Physics.CapsuleCast(
            bottom,
            top,
            radius,
            Vector3.up,
            castDistance,
            ~0,
            QueryTriggerInteraction.Ignore
            );
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _isRunning = context.performed;                                         // Sprint is active while the sprint input is being performed.
    }

    private void HandleGravity()
    {
        if (_isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = initialFallVelocity;                            // Keep the player lightly pushed toward the ground.
        }

        _verticalVelocity += gravity * Time.deltaTime;                          // Apply gravity over time.
    }

    private void HandleMovement()
    {
        // Convert local input direction into world-space movement based on where the camera is facing. 
        var move = cameraTransform.TransformDirection(new Vector3(_moveInput.x, 0, _moveInput.y)).normalized;

        // Choose movement speed based on the player's current state.
        var currentSpeed = _isCrouching ? crouchSpeed : _isRunning ? runSpeed : walkSpeed; 
        var finalMove = move * currentSpeed;

        // Add vertical movement from jumping and gravity.
        finalMove.y = _verticalVelocity; 

        var collisions = _characterController.Move(finalMove * Time.deltaTime);

        // If the palyer hits something above them, cancel update velocity.
        if ((collisions & CollisionFlags.Above) != 0)
        {
            _verticalVelocity = initialFallVelocity; 
        }
    }

    private void HandleCrouchTransition()
    {
        var currentHeight = _characterController.height;

        // Stop adjusting once the controller is close enough to the target height
        if (Mathf.Abs(currentHeight - _targetHeight) < 0.01f)
        {
            _characterController.height = _targetHeight; 
            return;
        }

        // Smoothly move the CharacterController height toward the target height.
        var newHeight = Mathf.Lerp(currentHeight, _targetHeight, crouchTransitionSpeed * Time.deltaTime);
        _characterController.height = newHeight;

        // Keep the controller centered around the player's position as the height changes. 
        _characterController.center = Vector3.up * (newHeight * 0.5f);

        // Move the camera to match the player's standing or crouching height.
        var cameraTargetPosition = cameraTransform.localPosition;
        cameraTargetPosition.y = _targetHeight - cameraOffset;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, cameraTargetPosition, crouchTransitionSpeed * Time.deltaTime); 
    }
}
