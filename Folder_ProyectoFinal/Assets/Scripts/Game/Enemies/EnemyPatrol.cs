using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enem Movement")]
    public Vector3 positionToMove;
    public float enemieSpeed;
    public SimpleLinkedList<NodoControl> allNodes;
    private NodoControl currentNode;
    public bool isInteracting = false;

    public float detectionRadius = 5f; 
    public Transform player;
    private bool isChasing = false;

    private void Start()
    {
        if (currentNode == null)
        {
            Debug.LogError("no hay nodo");
        }
        else
        {
            MoveToNextNode();
        }
    }
    private void Update()
    {
        if (isInteracting)
        {
            return;
        }

        if (isChasing)
        {
            positionToMove = player.position; 
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, positionToMove, enemieSpeed * Time.deltaTime);
        }

        Vector3 direction = positionToMove - transform.position;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, positionToMove) < 0.2f)
        {
            MoveToNextNode();
        }
    }

    public void SetInitialNodeEnemie(NodoControl initialNode)
    {
        if (initialNode != null)
        {
            currentNode = initialNode;
        }
        else
        {
            Debug.LogError("nodo inicial null");
        }
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        positionToMove = newPosition;
    }

    private void MoveToNextNode()
    {
        if (currentNode != null)
        {
            AdjacentNodeInfo nextNode = currentNode.GetRandomAdjacentNode();
            if (nextNode != null)
            {
                SetNewPosition(nextNode.node.transform.position);
                currentNode = nextNode.node;
            }
        }
    }

    public void StopPatrol()
    {
        isInteracting = true;
    }

    public void ResumePatrol()
    {
        isInteracting = false;
    }

    public void StartChasing()
    {
        isChasing = true; 
    }

    public void StopChasing()
    {
        isChasing = false; 
        MoveToNextNode();
    }
}