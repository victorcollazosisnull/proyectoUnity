using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class LaraCroftMovement : MonoBehaviour
{
    // Components
    private Rigidbody LaraRigidbody;
    private LaraCroftInputReader inputReader;
    private Animator LaraAnimator;

    [Header("Lara Croft Movement")]
    //------MOVEMENT------
    [SerializeField] private float LaraWalk;
    [SerializeField] public float LaraRun;
    [SerializeField] private bool LaraIsRunning = false;
    //------CROUNCHED------
    private bool LaraisCrouching = false; 
    private bool LaraisWalkingCrouched = false;
    //------CAMERA------
    [SerializeField] private float mouseSensitivity;
    public Transform cameraTransform;
    private Vector3 movementInput;
    private float mouseX, mouseY;
    private float xRotation = 0f;
    //------JUMP------
    [SerializeField] private float LarajumpForce = 5f;
    //------CAMERA AIM------
    private bool LaraIsAiming = false;
    // Events Animations
    public event Action<bool> OnMovementAnimation;
    public event Action OnJumpingAnimation;
    public event Action<bool> OnRunningAnimation;
    public event Action<bool> OnCrouchAnimation; 
    public event Action<bool> OnCrouchWalkingAnimation;

    [Header("Raycast Detection Floor")]
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Lara Croft Sounds")]
    private bool isRunning = false;
    private bool isMoving = false;

    [Header("Lara Croft Cameras")]
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera aimCamera;
    void Awake()
    {
        LaraRigidbody = GetComponent<Rigidbody>();
        LaraAnimator = GetComponent<Animator>();
        inputReader = FindAnyObjectByType<LaraCroftInputReader>();
    }

    private void Start() 
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    //------------Sub Input Reader--------------
    private void OnEnable() 
    {
        inputReader.OnMovementInput += Movement;
        inputReader.OnJumpInput += Jumping;
        inputReader.OnRunningInput += Running;
        inputReader.OnMouseInput += MouseMovement;
        inputReader.OnCrouchInput += Crouch;
        inputReader.OnAimInput += HandleAimInput;
        inputReader.OnAimInput += CamarasAimInput;
    }

    private void OnDisable() 
    {
        inputReader.OnMovementInput -= Movement;
        inputReader.OnJumpInput -= Jumping;
        inputReader.OnRunningInput -= Running;
        inputReader.OnMouseInput -= MouseMovement;
        inputReader.OnCrouchInput -= Crouch;
        inputReader.OnAimInput -= HandleAimInput;
        inputReader.OnAimInput -= CamarasAimInput;
    }
    //------------Movement3D--------------
    private void Movement(Vector2 input)
    {
        movementInput = new Vector3(input.x, 0, input.y);

        bool isWalking = movementInput != Vector3.zero && !LaraIsRunning && !LaraisCrouching;
        OnMovementAnimation?.Invoke(isWalking); // Events WalkAnimation

        bool isCrouchWalking = LaraisCrouching && movementInput != Vector3.zero;
        OnCrouchWalkingAnimation?.Invoke(isCrouchWalking); // Events CrounchedAnimation

        isMoving = movementInput != Vector3.zero;

        if (isMoving && isGrounded)
        {
            if (LaraIsRunning)
            {
                SFXManager.Instance.PlayRunningSound(); 
            }
            else
            {
                SFXManager.Instance.PlayWalkingSound(); 
            }
        }
        else
        {
            SFXManager.Instance.StopFootstepsSound(); 
        }
    }
    //------------Jump3D--------------
    private void Jumping()
    {
        if (isJumping || !isGrounded || LaraisCrouching || LaraIsAiming)
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
        if (LaraisCrouching || LaraIsAiming) 
        {
            isRunning = false;
        }
        LaraIsRunning = isRunning && movementInput != Vector3.zero;

        OnRunningAnimation?.Invoke(LaraIsRunning); // Events RunAnimation
    }
    //------------Crounched3D--------------
    private void Crouch()
    {
        if (LaraIsAiming)
        {
            return; 
        }
        else if (LaraisCrouching)
        {
            LaraisCrouching = false;
            OnCrouchAnimation?.Invoke(false); 
        }
        else
        {
            LaraisCrouching = true;
            OnCrouchAnimation?.Invoke(true); 
        }
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
    private void HandleAimInput(bool isAiming)
    {
        this.LaraIsAiming = isAiming;
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
    //-----RAYCAST-----
    private void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && isJumping)
        {
            isJumping = false;
            OnJumpingAnimation?.Invoke(); 
        }
    }

    private void CamarasAimInput(bool isAiming)
    {
        if (isAiming)
        {
            mainCamera.Priority = 1;
            aimCamera.Priority = 2;
        }
        else
        {
            mainCamera.Priority = 2;
            aimCamera.Priority = 1;
        }
    }
}