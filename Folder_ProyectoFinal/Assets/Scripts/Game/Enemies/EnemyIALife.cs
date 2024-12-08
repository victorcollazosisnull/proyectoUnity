using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class EnemyIALife : MonoBehaviour
{
    private EnemyIA enemyIA;  
    public static event Action OnDeath;
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyIA = GetComponent<EnemyIA>(); 
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        animator.SetTrigger("Die");
        OnDeath?.Invoke();
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(2f); 
        Destroy(gameObject);  
    }
}