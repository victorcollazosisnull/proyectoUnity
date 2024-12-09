using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

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

    [Header("GameOver")]
    public Image darkBackground;
    public GameObject gameOverPanel;
    public Button retryButton; 
    public float fadeDuration = 3f;
    [Header("Victory")]
    public Image background;
    public GameObject victoryPanel;
    public Button goToMenu;
    public float fade = 3f;
    private bool isVictory = false;
    private void Awake()
    {
        inputReader = GetComponent<LaraCroftInputReader>();
        animations = GetComponent<LaraCroftAnimations>();
        movement = GetComponent<LaraCroftMovement>();

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth;
        }

        gameOverPanel.SetActive(false);

        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RestartLevel); 
        }
    }

    private void OnEnable()
    {
        EnemyPatrol.OnPlayerDamage += TakeDamage;
    }

    private void OnDisable()
    {
        EnemyPatrol.OnPlayerDamage -= TakeDamage;

        if (retryButton != null)
        {
            retryButton.onClick.RemoveListener(RestartLevel);
        }
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
            StartCoroutine(HandleGameOver());
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
            StartCoroutine(HandleVictory());
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

    private IEnumerator HandleGameOver()
    {
        float elapsed = 0f;
        Color startColor = darkBackground.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            darkBackground.color = Color.Lerp(startColor, endColor, elapsed / fadeDuration);
            yield return null;
        }

        gameOverPanel.SetActive(true);
        EnableCursor(); 
    }
    private IEnumerator HandleVictory()
    {
        if (isVictory) yield break; 

        isVictory = true;

        float elapsed = 0f;
        Color startColor = background.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (elapsed < fade)
        {
            elapsed += Time.deltaTime;
            background.color = Color.Lerp(startColor, endColor, elapsed / fade);
            yield return null;
        }

        victoryPanel.SetActive(true);
        EnableCursor();
    }
    private void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene("ScenePrototypes");
    }
}