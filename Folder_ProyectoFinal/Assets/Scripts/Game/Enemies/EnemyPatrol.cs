using System;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Animator animator;
    public static event Action<float> OnPlayerDamage;
    [Header("Enemy Settings")]
    public float patrolSpeed = 1f;
    public float detectionRadius = 5f;
    public float stopChaseRadius = 7f;
    public float attackRadius = 1.5f;
    private bool isAttacking = false;
    public Transform player;
    [Header("MRUV")]
    public float initialSpeed = 4f; 
    public float maxSpeed = 10f; 
    public float acceleration = 0.5f;
    // Variables MRUV
    private float currentChaseSpeed;  
    private float chaseTime;
    [Header("Enemy Patrol")]
    public SimpleLinkedList<NodoControl> allNodes;
    private NodoControl currentNode;
    private Vector3 positionToMove;
    private bool isChasing = false;
    private bool isPatrolling = true;
    [Header("Attack Settings")]
    public float minAttackDistance = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        EnemieLife.OnDeath += StopPatrolling;
    }
    private void Update()
    {
        UpdateEnemyState();
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }
    private void OnDestroy()
    {
        EnemieLife.OnDeath -= StopPatrolling;
    }
    public void SetInitialNode(NodoControl initialNode)
    {
        currentNode = initialNode;
        SetNewPosition(initialNode.transform.position);
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        positionToMove = newPosition;
    }

    private void Patrol()
    {
        Vector3 targetPosition = new Vector3(positionToMove.x, transform.position.y, positionToMove.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, patrolSpeed * Time.deltaTime);

        LookAtMoveDirection(positionToMove);

        if (Vector3.Distance(transform.position, positionToMove) < 0.2f)
        {
            MoveToNextNode();
        }

        CheckPlayerDistance();
    }
    public void StopPatrolling()
    {
        isPatrolling = false;  
    }
    private void MoveToNextNode()
    {
        AdjacentNodeInfo nextNode = currentNode.GetRandomAdjacentNode();
        if (nextNode != null)
        {
            SetNewPosition(nextNode.node.transform.position);
            currentNode = nextNode.node;
        }
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            if (!isChasing)
            {
                StartChase(); 
            }
        }
        else if (distanceToPlayer > stopChaseRadius)
        {
            StopChase(); 
        }
    }
    public void StartChase()
    {
        isChasing = true;
        chaseTime = 0f;
        currentChaseSpeed = initialSpeed; 
    }

    public void StopChase()
    {
        isChasing = false;
        chaseTime = 0f; 
        currentChaseSpeed = initialSpeed;
        SetNewPosition(currentNode.transform.position);
    }

    private void ChasePlayer()
    {
        chaseTime += Time.deltaTime;
        currentChaseSpeed = Mathf.Min(initialSpeed + Mathf.Pow(acceleration * chaseTime, 0.5f), maxSpeed);

        Vector3 directionToPlayer = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;

        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= minAttackDistance)
        {
            if (!isAttacking)  
            {
                StartAttack();  
            }
            transform.position = Vector3.MoveTowards(transform.position, transform.position, 0); 
        }
        else if (distanceToPlayer > minAttackDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, currentChaseSpeed * Time.deltaTime);
        }

        LookAtMoveDirection(player.position);

        if (distanceToPlayer > stopChaseRadius)
        {
            StopChase(); 
        }
    }
    private void ResetChaseSpeed()
    {
        chaseTime = 0f; 
        currentChaseSpeed = initialSpeed; 
    }

    private void LookAtMoveDirection(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; 
        if (direction.sqrMagnitude > 0.01f) 
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    private void UpdateEnemyState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius && distanceToPlayer > minAttackDistance && !isAttacking)
        {
            StartAttack();
        }
        else if (distanceToPlayer <= detectionRadius && distanceToPlayer > attackRadius)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > stopChaseRadius)
        {
            isChasing = false;
        }
    }
    private void StartAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        isChasing = false;

        Invoke("OnAttackHit", 0.5f); 
    }

    private void OnAttackHit()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRadius) 
        {
            OnPlayerDamage?.Invoke(1f);
            Debug.Log("me ataco");
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
    private void ReturnToPatrol()
    {
        animator.SetTrigger("Walk"); 
        isChasing = false;
    }
}