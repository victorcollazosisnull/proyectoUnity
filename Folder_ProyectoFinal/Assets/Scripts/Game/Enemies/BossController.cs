using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float summonCooldown = 10f;
    public int enemiesPerWave = 5;
    private Animator animator;

    private bool isSummoning = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SummonEnemiesRoutine());
        }
    }

    private IEnumerator SummonEnemiesRoutine()
    {
        while (true)
        {
            if (!isSummoning)
            {
                isSummoning = true;
                animator.SetBool("Invoke", true);

                yield return new WaitForSeconds(1f);
                SummonEnemies();

                animator.SetBool("Invoke", false);
                isSummoning = false;
            }
            yield return new WaitForSeconds(summonCooldown);
        }
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
                    enemyAI.StartChase(); 
                }
            }
        }
    }
}