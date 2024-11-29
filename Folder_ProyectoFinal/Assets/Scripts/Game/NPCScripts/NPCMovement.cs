using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("NPC Movement")]
    public Vector3 positionToMove;
    public float npcSpeed;
    public SimpleLinkedList<NodoControl> allNodes;
    private NodoControl currentNode;
    public bool isInteracting = false;

    private void Update()
    {
        if (isInteracting)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, positionToMove, npcSpeed * Time.deltaTime);

        Vector3 direction = positionToMove - transform.position;
        direction.y = 0; 
        if (direction.sqrMagnitude > 0.1f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); 
        }

        if ((transform.position - positionToMove).sqrMagnitude < 0.1f * 0.1f)
        {
            MoveToNextNode();
        }
    }

    public void SetInitialNode(NodoControl initialNode)
    {
        currentNode = initialNode; 
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        positionToMove = newPosition;
    }

    private void MoveToNextNode()
    {
        AdjacentNodeInfo nextNode = currentNode.GetRandomAdjacentNode();

        if (nextNode != null)
        {
            SetNewPosition(nextNode.node.transform.position);
            currentNode = nextNode.node;
        }
        else
        {
            Debug.LogWarning($"nodo actual ({currentNode.name}) no tiene vecinos");

            NodoControl fallbackNode = FindFallbackNode();
            if (fallbackNode != null)
            {
                SetNewPosition(fallbackNode.transform.position);
                currentNode = fallbackNode;
            }
            else
            {
                Debug.LogError("me bugeo porque no hay vecinos");
            }
        }
    }
    private NodoControl FindFallbackNode()
    {
        for (int i = 0; i < allNodes.Count(); i++)
        {
            NodoControl node = allNodes.Get(i);
            if (node != currentNode && node.adjacentNodes.Count() > 0)
            {
                return node;
            }
        }
        return null;
    }
    public void StopPatrol()
    {
        isInteracting = true;
    }

    public void ResumePatrol()
    {
        isInteracting = false;
    }
}