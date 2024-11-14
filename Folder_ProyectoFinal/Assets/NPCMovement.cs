using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Vector3 positionToMove;
    public float npcSpeed;
    public SimpleLinkedList<NodoControl> allNodes;
    private NodoControl currentNode;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, positionToMove, npcSpeed * Time.deltaTime);

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
        AdjacentNodeInfo nextNodeInfo = currentNode.GetRandomAdjacentNode();
        if (nextNodeInfo != null)
        {
            SetNewPosition(nextNodeInfo.node.transform.position);
            currentNode = nextNodeInfo.node; 
        }
    }
}