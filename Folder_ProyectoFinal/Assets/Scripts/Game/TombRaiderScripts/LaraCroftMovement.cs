using System.Collections;
using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.VisualScripting;

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
    public bool canMove = true;
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
    public event Action<bool> OnJumpingAnimation;
    public event Action<bool> OnRunningAnimation;
    public event Action<bool> OnCrouchAnimation; 
    public event Action<bool> OnCrouchWalkingAnimation;
    public event Action<bool> OnBowAimAnimation; 
    public event Action OnBowShootAnimation;
    //------ARROW------
    public bool LaraHasBow = false; 
    //-----NPC------
    private bool LaraIsInteracting;
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
        inputReader = GetComponent<LaraCroftInputReader>();
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
        inputReader.OnCrouchInput += Crouch;
        inputReader.OnAimInput += HandleAimInput;
        inputReader.OnAimInput += CamarasAimInput;
        inputReader.OnAttackInput += HandleAttackInput;
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
        inputReader.OnAttackInput -= HandleAttackInput;
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
    }
    //------------Jump3D--------------
    private void Jumping()
    {
        if (isJumping || !isGrounded || LaraisCrouching || LaraIsAiming)
        {
            return;
        }
        OnJumpingAnimation?.Invoke(true);
        isJumping = true;
        isGrounded = false;
        LaraAnimator.SetBool("LaraIsJumping", false); 
        LaraRigidbody.AddForce(Vector3.up * LarajumpForce, ForceMode.Impulse);
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
        if (canMove)
        {
            
        }
    }
    private void HandleAimInput(bool isAiming)
    {

        this.LaraIsAiming = isAiming;

        if (LaraHasBow)
        {
            OnBowAimAnimation?.Invoke(isAiming); 
            LaraAnimator.SetBool("LaraIsAimingBow", isAiming); 

            CamarasAimInput(isAiming);
        }
    }

    private void HandleAttackInput()
    {
        if (LaraHasBow && LaraIsAiming)
        {
            OnBowShootAnimation?.Invoke(); 
            LaraAnimator.SetTrigger("LaraShootBow");
        }
    }
    public void EquipBow(bool hasBow)
    {
        LaraHasBow = hasBow;
        if (hasBow)
        {
            Debug.Log("Lara ahora tiene el arco .");
        }
        else
        {
            Debug.Log("Lara DEJO de usar el arco.");
        }
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
        bool wasGrounded = isGrounded;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && !wasGrounded) 
        {
            isJumping = false;
            LaraAnimator.SetBool("LaraIsJumping", false);
        }
        else if (!isGrounded && wasGrounded) 
        {
            LaraAnimator.SetBool("LaraIsJumping", true);
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
    public void SetAnimationState(string animationName, bool state)
    {
        LaraAnimator.SetBool(animationName, state);
    }
    public void StopMovement()
    {
        canMove = false;
        LaraAnimator.SetBool("LaraIsWalking", false);
        LaraAnimator.SetBool("LaraIsRunning", false);
    }

    public void ResumeMovement()
    {
        canMove = true;
    }
}