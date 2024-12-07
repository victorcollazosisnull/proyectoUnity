using UnityEngine;

public class LaraCroftHealth : MonoBehaviour
{
    public float maxHealth = 8f;
    public float currentHealth = 8f;
    public BarLifePlayerUI healthBar;
    private LaraCroftAnimations animations;

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
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.RestarVida(currentHealth);  

        if (currentHealth == 0)
        {
            Debug.Log("mori porque toy bugeado");
            PlayDieAnimation();  
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
}