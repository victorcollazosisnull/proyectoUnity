using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputReader : MonoBehaviour
{
    public event Action<Vector2> OnMovement;
    public event Action OnJump;
    public event Action<bool> OnRunning;
    public event Action<Vector2> OnMouseInput;

    public void ReadDirection(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        OnMovement?.Invoke(input);
    }
    public void ReadJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJump?.Invoke();
        }
    }
    public void ReadRun(InputAction.CallbackContext context)
    {
        bool isRunning = context.performed;
        OnRunning?.Invoke(isRunning);
    }
    public void ReadMouseInput(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        OnMouseInput?.Invoke(lookInput);
    }
}