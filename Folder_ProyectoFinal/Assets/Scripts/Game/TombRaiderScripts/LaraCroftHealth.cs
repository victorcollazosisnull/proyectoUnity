using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LaraCroftHealth : MonoBehaviour
{
    public float maxHealth = 8f;
    public float currentHealth = 8f;
    private BarLifePlayerUI healthBar;
    public event Action OnDieAnimation;
    [SerializeField] private bool LaraIsDead = false;
    private Animator animator;
    private LaraCroftInputReader inputReader;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputReader = GetComponent<LaraCroftInputReader>();
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
        currentHealth -= damageAmount;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            healthBar.RestarVida(currentHealth);
        }
    }
    private void Die()
    {
        if (currentHealth == 0)
        {
            OnDieAnimation?.Invoke();
            inputReader.BlockInputs(true);
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