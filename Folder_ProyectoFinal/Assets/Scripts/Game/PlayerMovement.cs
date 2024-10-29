using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento del jugador")]
    private Rigidbody _rigidbody;
    public float walk;
    public float run;
    private bool isRunning = false;
    public float mouseSensitivity;
    public Transform cameraTransform;
    private bool isCrouching = false;
    public float crouchSpeed = 1.5f;
    private Vector3 movementInput;
    private float mouseX, mouseY;
    private float xRotation = 0f;
    public float jumpForce = 5f;

    public InputReader inputReader;
    private Animator animator;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputReader = FindAnyObjectByType<InputReader>();
    }
    private void OnEnable()
    {
        inputReader.OnMovement += Movement;
        inputReader.OnJump += Jumping;
        inputReader.OnRunning += Running;
        inputReader.OnMouseInput += MouseMovement;
    }
    private void OnDisable()
    {
        inputReader.OnMovement -= Movement;
        inputReader.OnJump -= Jumping;
        inputReader.OnRunning -= Running;
        inputReader.OnMouseInput -= MouseMovement;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Movement(Vector2 input)
    {
        movementInput = new Vector3(input.x, 0, input.y);
    }
    private void Jumping()
    {
        if (movementInput != Vector3.zero)
        {
            Jump();
            animator.SetTrigger("JumpRun");
        }
        else
        {
            Jump();
            animator.SetTrigger("JumpIdle");
        }
    }
    private void Running(bool isRunning)
    {
        this.isRunning = isRunning;
        animator.SetBool("Running", isRunning);
    }
    private void MouseMovement(Vector2 lookInput)
    {
        mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
    }
    private void Update()
    {
        RotateCamera();
    }
    private void Jump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void RotateCamera()
    {
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -10f, 20f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    private void AnimationsControl()
    {
        bool isMoving = movementInput != Vector3.zero;
        animator.SetBool("Walking", isMoving && !isRunning);
        animator.SetBool("Running", isRunning && isMoving);
    }
    private void FixedUpdate()
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        float currentSpeed;
        if (isRunning && movementInput != Vector3.zero) 
        {
            currentSpeed = run;
        }
        else
        {
            currentSpeed = walk;
        }

        Vector3 movement = (forward * movementInput.z + right * movementInput.x) * currentSpeed;
        movement.y = _rigidbody.velocity.y;
        _rigidbody.velocity = movement;

        AnimationsControl();
    }
}