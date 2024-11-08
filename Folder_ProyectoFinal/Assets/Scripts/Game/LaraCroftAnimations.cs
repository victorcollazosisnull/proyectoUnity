using UnityEngine;

public class LaraCroftAnimations : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        LaraCroftMovement playerMovement = GetComponent<LaraCroftMovement>();
        playerMovement.OnMovementAnimation += WalkingAnimation;
        playerMovement.OnJumpingAnimation += JumpAnimation;
        playerMovement.OnRunningAnimation += RunningAnimation;
    }

    private void OnDisable()
    {
        LaraCroftMovement playerMovement = GetComponent<LaraCroftMovement>();
        playerMovement.OnMovementAnimation -= WalkingAnimation;
        playerMovement.OnJumpingAnimation -= JumpAnimation;
        playerMovement.OnRunningAnimation -= RunningAnimation;
    }

    private void WalkingAnimation(bool isWalking)
    {
        animator.SetBool("LaraIsWalking", isWalking);
    }

    private void JumpAnimation(bool isRunning)
    {
        if (isRunning)
        {
            animator.SetTrigger("LaraIsJumpAndRunning");
        }
        else
        {
            animator.SetTrigger("LaraIsJump");
        }
    }

    private void RunningAnimation(bool isRunning)
    {
        animator.SetBool("LaraIsRunning", isRunning);
    }
}