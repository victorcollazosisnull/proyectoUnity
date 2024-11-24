using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float patrolSpeed = 2f;
    public float detectionRadius = 5f;
    public float stopChaseRadius = 7f;
    public Transform player;

    [Header("Enemy Patrol")]
    public SimpleLinkedList<NodoControl> allNodes;
    private NodoControl currentNode;
    private Vector3 positionToMove;
    private bool isChasing = false;

    private void Update()
    {
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
        if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            isChasing = true;
        }
        if (Vector3.Distance(transform.position, player.position) > stopChaseRadius)
        {
            isChasing = false;
            SetNewPosition(currentNode.transform.position);
        }
    }

    private void ChasePlayer()
    {
        Vector3 directionToPlayer = new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.position, patrolSpeed * Time.deltaTime);  
        LookAtMoveDirection(player.position);

        if (Vector3.Distance(transform.position, player.position) > stopChaseRadius)
        {
            isChasing = false;
            SetNewPosition(currentNode.transform.position);
        }
    }

    private void LookAtMoveDirection(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); 
        }
    }
}