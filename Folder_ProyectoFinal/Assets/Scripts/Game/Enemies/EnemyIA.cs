using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private Transform player;
    public int maxHits = 3; 
    private int currentHits = 0; 
    public float chaseSpeed = 3f; 

    private Animator animator;
    private bool isChasing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        currentHits++; 

        if (currentHits >= maxHits)
        {
            Die(); 
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");

        Destroy(gameObject, 2f); 
    }

    public void SetPlayer(Transform target)
    {
        player = target;
    }

    public void StartChasingPlayer()
    {
        isChasing = true;
    }

    private void Update()
    {
        if (isChasing && player != null)
        {
            ChasePlayer(); 
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
}