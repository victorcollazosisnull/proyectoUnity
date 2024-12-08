using System;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private Animator animator;
    private bool isChasing = false;
    private bool isAttacking = false;
    private Transform player;
    private EnemieLife enemyLife;

    [Header("Enemy Settings")]
    public float chaseSpeed = 3f;
    public float attackRadius = 1.5f;

    public static event Action<float> OnPlayerDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyLife = GetComponent<EnemieLife>();
        if (enemyLife != null)
        {
            EnemieLife.OnDeath += HandleDeath;
        }
    }

    public void SetPlayer(Transform target)
    {
        player = target;
        StartChasingPlayerDelayed(); 
    }

    private void OnDestroy()
    {
        if (enemyLife != null)
        {
            EnemieLife.OnDeath -= HandleDeath;
        }
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

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRadius && !isAttacking)
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        Invoke("OnAttackHit", 0.5f); 
    }

    public void StartChasingPlayerDelayed()
    {
        isChasing = true;
        animator.SetTrigger("Walk");
    }

    private void OnAttackHit()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius)
        {
            OnPlayerDamage?.Invoke(1f); 
            Debug.Log("Enemy attacked the player");
        }
        else
        {
            EndAttack();
        }
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void HandleDeath()
    {
        isChasing = false;
        Destroy(gameObject); 
    }
}