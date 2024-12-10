using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;

public class LaraCroftHealth : MonoBehaviour
{
    public float maxHealth = 8f;
    public float currentHealth = 8f;
    public BarLifePlayerUI healthBar;
    private LaraCroftAnimations animations;
    private LaraCroftInputReader inputReader;
    private LaraCroftMovement movement;
    private EnemyPatrol enemyPatrol;
    private bool isInvulnerable = false;
    public float invulnerabilityTime = 0.5f;

    public static event Action OnGameOver;
    public static event Action OnVictory;
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

    public void TakeDamage(float damageAmount)
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
            OnGameOver?.Invoke();
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
        else if (other.gameObject.CompareTag("win"))
        {
            HandleVictory();
        }
    }
    private void HandleVictory()
    {
        movement.StopMovement();
        OnVictory?.Invoke();
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
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("ScenePrototypes");
    }
}