using System;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private Transform player;
    private bool isChasing = false;
    private bool isAttacking = false;
    private Animator animator;

    public static event Action<float> OnPlayerDamage;

    [Header("Enemy Settings")]
    public float chaseSpeed = 3f;
    public float detectionRadius = 5f;
    public float attackRadius = 1.5f;
    public float stopChaseRadius = 7f;
    public float minAttackDistance = 1.0f;
    public float health = 100f;  

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlayer(Transform target)
    {
        player = target;
    }

    private void Update()
    {
        if (player != null && health > 0)  
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius && !isChasing)
            {
                StartChase();
            }
            if (isChasing)
            {
                ChasePlayer();

                if (distanceToPlayer <= attackRadius && !isAttacking)
                {
                    StartAttack();
                }
                else if (distanceToPlayer > attackRadius && !isAttacking)
                {
                    animator.SetTrigger("Walk");  
                }
            }
        }
        else if (health <= 0)
        {
            Die(); 
        }
    }

    public void StartChase()
    {
        isChasing = true;
        animator.SetTrigger("Walk"); 
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        Invoke("OnAttackHit", 0.5f);
    }

    private void OnAttackHit()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            OnPlayerDamage?.Invoke(1f);
            Debug.Log("El enemigo ha atacado al jugador.");
        }

        EndAttack();
    }

    private void EndAttack()
    {
        isAttacking = false;
        if (Vector3.Distance(transform.position, player.position) > attackRadius)
        {
            animator.SetTrigger("Walk");  
            isChasing = true;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; 
        if (health <= 0)
        {
            Die();  
        }
    }

    private void Die()
    {
        animator.SetTrigger("Die");  
        Destroy(gameObject, 1f); 
        Debug.Log("El enemigo ha muerto.");
    }
}