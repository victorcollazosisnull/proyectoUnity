using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemieLife : MonoBehaviour
{
    public static event Action OnDeath;
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthSlider;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    private void Die()
    {
        animator.SetTrigger("Die");
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); 
    }
}