using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LaraCroftInputReader : MonoBehaviour
{
    // Events Inputs
    public event Action<Vector2> OnMovementInput;
    public event Action OnJumpInput;
    public event Action<bool> OnRunningInput;
    public event Action<Vector2> OnMouseInput;
    public event Action OnCrouchInput;
    public event Action<bool> OnAimInput;

    public void ReadDirection(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        OnMovementInput?.Invoke(input);
    }
    public void ReadJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpInput?.Invoke();
        }
    }
    public void ReadRun(InputAction.CallbackContext context)
    {
        bool isRunning = context.performed;
        OnRunningInput?.Invoke(isRunning);
    }
    public void ReadCrouch(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            OnCrouchInput?.Invoke(); 
        }
    }
    public void ReadMouseInput(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        OnMouseInput?.Invoke(lookInput);
    }
    public void ReadAim(InputAction.CallbackContext context)
    {
        bool isAiming = context.performed;
        OnAimInput?.Invoke(isAiming); 
    }
}