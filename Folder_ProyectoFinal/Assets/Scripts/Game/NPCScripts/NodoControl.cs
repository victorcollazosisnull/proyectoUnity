using UnityEngine;

public class NodoControl : MonoBehaviour
{
    public SimpleLinkedList<AdjacentNodeInfo> adjacentNodes; 

    private void Awake()
    {
        adjacentNodes = new SimpleLinkedList<AdjacentNodeInfo>();
    }

    public void SetRandomAdjacentNodes(SimpleLinkedList<NodoControl> allNodes)
    {
        int numberOfConnections = Random.Range(1, 4);

        for (int i = 0; i < numberOfConnections; i++)
        {
            NodoControl randomNode = allNodes.Get(Random.Range(0, allNodes.Count()));

            if (randomNode != this && !IsAlreadyConnected(randomNode))
            {
                AddAdjacentNode(randomNode, Random.Range(1f, 4f));
                randomNode.AddAdjacentNode(this, Random.Range(1f, 4f)); 
            }
        }

        if (adjacentNodes.Count() == 0 && allNodes.Count() > 1)
        {
            NodoControl fallbackNode = allNodes.Get(Random.Range(0, allNodes.Count()));
            if (fallbackNode != this)
            {
                AddAdjacentNode(fallbackNode, Random.Range(1f, 4f));
                fallbackNode.AddAdjacentNode(this, Random.Range(1f, 4f));
            }
        }
    }
    private bool IsAlreadyConnected(NodoControl node)
    {
        for (int i = 0; i < adjacentNodes.Count(); i++)
        {
            if (adjacentNodes.Get(i).node == node)
                return true;
        }
        return false;
    }

    public void AddAdjacentNode(NodoControl node, float weight)
    {
        adjacentNodes.Add(new AdjacentNodeInfo(node, weight));
    }

    public AdjacentNodeInfo GetRandomAdjacentNode()
    {
        if (adjacentNodes.Count() > 0)
        {
            return adjacentNodes.Get(Random.Range(0, adjacentNodes.Count()));
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < adjacentNodes.Count(); i++)
        {
            var nodeInfo = adjacentNodes.Get(i);
            if (nodeInfo != null && nodeInfo.node != null)
            {
                Gizmos.DrawLine(transform.position, nodeInfo.node.transform.position);
            }
        }
    }
}
public class AdjacentNodeInfo
{
    public NodoControl node;
    public float weight;

    public AdjacentNodeInfo(NodoControl node, float weight)
    {
        this.node = node;
        this.weight = weight;
    }
}