using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaraCroftHealth : MonoBehaviour
{
    public float maxHealth = 8f;
    public float currentHealth = 8f;
    public BarLifePlayerUI healthBar;

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
            healthBar.RestarVida(currentHealth);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("brazo"))
        {
            TakeDamage(currentHealth);
        }
    }
}