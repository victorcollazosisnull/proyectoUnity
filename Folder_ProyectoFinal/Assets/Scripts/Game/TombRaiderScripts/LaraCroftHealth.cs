using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class LaraCroftHealth : MonoBehaviour
{
    public float maxHealth = 8f;
    public float currentHealth = 8f;
    public BarLifePlayerUI healthBar;
    private LaraCroftAnimations animations;
    private LaraCroftInputReader inputReader;
    private LaraCroftMovement movement;

    private bool isInvulnerable = false; 
    public float invulnerabilityTime = 0.5f; 

    private void Awake()
    {
        inputReader = GetComponent<LaraCroftInputReader>();
        animations = GetComponent<LaraCroftAnimations>();
        movement = GetComponent<LaraCroftMovement>();
        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }
    }

    private void OnEnable()
    {
        EnemyPatrol.OnPlayerDamage += TakeDamage;
    }

    private void OnDisable()
    {
        EnemyPatrol.OnPlayerDamage -= TakeDamage;
    }

    private void TakeDamage(float damageAmount)
    {
        if (isInvulnerable) return;

        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.RestarVida(currentHealth);

        if (currentHealth == 0)
        {
            Debug.Log("mori");
            PlayDieAnimation();
            movement.StopMovement();
            inputReader.BlockInputs(true);
        }
        else
        {
            StartCoroutine(InvulnerabilityCoroutine()); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("brazo"))
        {
            TakeDamage(1f);
        }
    }

    private void PlayDieAnimation()
    {
        if (animations != null)
        {
            animations.PlayDieAnimation();
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvulnerable = false;
    }
    public void UseMedKit()
    {
        if (currentHealth < maxHealth) 
        {
            currentHealth += 2f; 
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth; 
            }
            healthBar.RestarVida(currentHealth); 
        }
    }
    public void UsePotion()
    {
        currentHealth = maxHealth;
        healthBar.RestarVida(currentHealth); 
        Debug.Log("Salud restaurada como debe ser");
    }
}