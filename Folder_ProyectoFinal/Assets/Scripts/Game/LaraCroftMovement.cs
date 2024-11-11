using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaraCroftMovement : MonoBehaviour
{
    // Components
    private Rigidbody LaraRigidbody;
    private LaraCroftInputReader inputReader;
    private Animator LaraAnimator;

    [Header("Lara Croft Movement")]
    [SerializeField] private float LaraWalk;
    [SerializeField] public float LaraRun;
    [SerializeField] private bool LaraIsRunning = false;
    [SerializeField] private float mouseSensitivity;
    public Transform cameraTransform;
    private Vector3 movementInput;
    private float mouseX, mouseY;
    private float xRotation = 0f;
    [SerializeField] private float LarajumpForce = 5f;

    // Events Animations
    public event Action<bool> OnMovementAnimation;
    public event Action OnJumpingAnimation;
    public event Action<bool> OnRunningAnimation; 

    [Header("Raycast Detection Floor")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask; 

    void Awake()
    {
        LaraRigidbody = GetComponent<Rigidbody>();
        LaraAnimator = GetComponent<Animator>();
        inputReader = FindAnyObjectByType<LaraCroftInputReader>();
    }

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //------------Sub Input Reader--------------
    private void OnEnable() 
    {
        inputReader.OnMovementInput += Movement;
        inputReader.OnJumpInput += Jumping;
        inputReader.OnRunningInput += Running;
        inputReader.OnMouseInput += MouseMovement;
    }

    private void OnDisable() 
    {
        inputReader.OnMovementInput -= Movement;
        inputReader.OnJumpInput -= Jumping;
        inputReader.OnRunningInput -= Running;
        inputReader.OnMouseInput -= MouseMovement;
    }
    //------------Movement3D--------------
    private void Movement(Vector2 input)
    {
        movementInput = new Vector3(input.x, 0, input.y);

        bool isWalking = movementInput != Vector3.zero && !LaraIsRunning; 
        OnMovementAnimation?.Invoke(isWalking); // Events WalkAnimation
    }
    //------------Jump3D--------------
    private void Jumping()
    {
        if (isJumping || !isGrounded)
        {
            return;
        }
        OnJumpingAnimation?.Invoke(); // Events JumpAnimation
        isJumping = true;
        Jump();  
        isGrounded = false;
    }
    //------------Run3D--------------
    private void Running(bool isRunning)
    {
        LaraIsRunning = isRunning && movementInput != Vector3.zero;

        OnRunningAnimation?.Invoke(LaraIsRunning); // Events RunAnimation
    }
    //------------Mouse3D--------------
    private void MouseMovement(Vector2 lookInput)
    {
        mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
    }

    private void Update()
    {
        RotateCamera();
        CheckGroundStatus(); 
    }

    private void Jump()
    {
        LaraRigidbody.AddForce(Vector3.up * LarajumpForce, ForceMode.Impulse);
    }

    private void RotateCamera()
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -10f, 20f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        float currentSpeed = LaraIsRunning && movementInput != Vector3.zero ? LaraRun : LaraWalk;
        Vector3 movement = (forward * movementInput.z + right * movementInput.x) * currentSpeed;
        movement.y = LaraRigidbody.velocity.y;
        LaraRigidbody.velocity = movement;
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && isJumping)
        {
            isJumping = false;
            OnJumpingAnimation?.Invoke(); 
        }
    }
}