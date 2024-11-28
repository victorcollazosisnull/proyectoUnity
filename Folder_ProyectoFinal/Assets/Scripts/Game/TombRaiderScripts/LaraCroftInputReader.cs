using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class LaraCroftInputReader : MonoBehaviour
{
    public LaraCroftMovement laraMovement;
    public NPCInteraction npcInteraction;
    private bool isInteractionBlocked = false;
    // Events Inputs
    public event Action<Vector2> OnMovementInput;
    public event Action OnJumpInput;
    public event Action<bool> OnRunningInput;
    public event Action<Vector2> OnMouseInput;
    public event Action OnCrouchInput;
    public event Action<bool> OnAimInput;
    public event Action<float> OnMouseWheelInput;
    public event Action OnAttackInput;
    public void BlockInputs(bool block)
    {
        isInteractionBlocked = block;
    }
    public void ReadDirection(InputAction.CallbackContext context)
    {
        if (isInteractionBlocked) return;
        Vector2 input = context.ReadValue<Vector2>();
        OnMovementInput?.Invoke(input);
    }
    public void ReadJump(InputAction.CallbackContext context)
    {
        if (isInteractionBlocked) return;
        if (context.performed)
        {
            OnJumpInput?.Invoke();
        }
    }
    public void ReadRun(InputAction.CallbackContext context)
    {
        if (isInteractionBlocked) return;
        bool isRunning = context.performed;
        OnRunningInput?.Invoke(isRunning);
    }
    public void ReadCrouch(InputAction.CallbackContext context)
    {
        if (isInteractionBlocked) return;
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
    public void ReadMouseWheel(InputAction.CallbackContext context)
    {
        float scrollInput = context.ReadValue<Vector2>().y;
        OnMouseWheelInput?.Invoke(scrollInput);
    }
    public void ReadInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            npcInteraction.Interacting(); 
        }
    }
    public void ReadAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackInput?.Invoke();
        }
    }
}