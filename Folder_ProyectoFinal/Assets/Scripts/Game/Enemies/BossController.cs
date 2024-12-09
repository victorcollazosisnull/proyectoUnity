using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public Transform player;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float summonCooldown = 5f; 
    public int enemiesPerWave = 3;
    private Animator animator;
    public GameObject win;
    private bool isSummoning = false;
    private int currentHits = 0;
    public int maxHits = 100; 

    public Slider healthSlider;
    private int maxHealth = 100;
    private int currentHealth;

    public Canvas victoryCanvas; 
    public Image fadeImage; 
    public Text victoryText; 
    public Button backToMenuButton;
    private void Awake()
    {
        win.gameObject.SetActive(false);
        animator = GetComponent<Animator>();
        currentHealth = maxHits;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHits;
            healthSlider.value = currentHealth;
            healthSlider.gameObject.SetActive(false); 
        }
        victoryCanvas.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(false);
        victoryText.gameObject.SetActive(false);
        backToMenuButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            if (healthSlider != null)
            {
                healthSlider.gameObject.SetActive(true);
                StartCoroutine(SummonEnemiesRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Jugador detectado");
            if (healthSlider != null)
            {
                healthSlider.gameObject.SetActive(false);
            }

            StopCoroutine(SummonEnemiesRoutine());
            isSummoning = false; 
        }
    }

    private IEnumerator SummonEnemiesRoutine()
    {
        isSummoning = true;
        animator.SetBool("Invoke", true);

        SummonEnemies();
        yield return new WaitForSeconds(summonCooldown);
        isSummoning = false;

        
        yield return null;    
    }

    private void SummonEnemies()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            if (spawnPoints.Length > 0)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                EnemyIA enemyAI = enemy.GetComponent<EnemyIA>();
                if (enemyAI != null)
                {
                    enemyAI.SetPlayer(player);
                    enemyAI.StartChasingPlayer();
                }
            }
        }
    }

    public void TakeDamage()
    {
        currentHits++; 

        currentHealth -= 1;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
        win.gameObject.SetActive(true);
    }
}