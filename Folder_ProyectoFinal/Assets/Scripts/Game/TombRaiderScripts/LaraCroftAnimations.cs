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
        playerMovement.OnCrouchAnimation += CrouchAnimation;
        playerMovement.OnCrouchWalkingAnimation += CrouchWalkingAnimation;
        playerMovement.OnBowAimAnimation += BowAimAnimation;
        playerMovement.OnBowShootAnimation += BowShootAnimation;
    }

    private void OnDisable()
    {
        LaraCroftMovement playerMovement = GetComponent<LaraCroftMovement>();
        playerMovement.OnMovementAnimation -= WalkingAnimation;
        playerMovement.OnJumpingAnimation -= JumpAnimation;
        playerMovement.OnRunningAnimation -= RunningAnimation;
        playerMovement.OnCrouchAnimation -= CrouchAnimation;
        playerMovement.OnCrouchWalkingAnimation -= CrouchWalkingAnimation;
        playerMovement.OnBowAimAnimation -= BowAimAnimation;
        playerMovement.OnBowShootAnimation -= BowShootAnimation;
    }

    private void WalkingAnimation(bool isWalking)
    {
        animator.SetBool("LaraIsWalking", isWalking);
    }

    private void JumpAnimation(bool isJumping)
    {
        animator.SetBool("LaraIsJumping", isJumping);
    }

    private void RunningAnimation(bool isRunning)
    {
        animator.SetBool("LaraIsRunning", isRunning);
    }

    private void CrouchAnimation(bool isCrouching)
    {
        animator.SetBool("LaraIsCrounched", isCrouching);  
    }

    private void CrouchWalkingAnimation(bool isCrouchWalking)
    {
        animator.SetBool("LaraIsWalkingCrounched", isCrouchWalking);
    }

    private void BowAimAnimation(bool isAiming)
    {
        animator.SetBool("LaraIsAimingBow", isAiming);
    }

    private void BowShootAnimation()
    {
        animator.SetTrigger("LaraShootBow");
    }
}