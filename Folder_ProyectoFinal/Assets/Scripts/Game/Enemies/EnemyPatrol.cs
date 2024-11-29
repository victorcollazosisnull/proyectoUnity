using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float patrolSpeed = 1f;
    public float detectionRadius = 5f;
    public float stopChaseRadius = 7f;
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
        chaseTime = 0f; // Reinicia time
        currentChaseSpeed = initialSpeed; // Reinicia V initial
    }

    public  void StopChase()
    {
        isChasing = false;
        chaseTime = 0f; // Reseteo time
        currentChaseSpeed = initialSpeed; // Reseteo speed
        SetNewPosition(currentNode.transform.position);
    }

    private void ChasePlayer()
    {
        chaseTime += Time.deltaTime;

        currentChaseSpeed = Mathf.Min(initialSpeed + acceleration * chaseTime, maxSpeed);

        Vector3 directionToPlayer = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.position, currentChaseSpeed * Time.deltaTime);

        LookAtMoveDirection(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 1.5f) 
        {
            ResetChaseSpeed(); 
        }

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

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > stopChaseRadius)
        {
            isChasing = false;
        }
    }
}