using System.Collections;
using System.Collections.Generic;
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
            currentHealth = 0;  // La vida nunca debe ser menor que 0
        }

        // Actualizar la barra de vida con el nuevo valor
        healthBar.RestarVida(currentHealth);

        // Verificar si la vida llega a 0 para activar la animación de muerte
        if (currentHealth == 0)
        {
            PlayDieAnimation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("brazo"))
        {
            TakeDamage(currentHealth);
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