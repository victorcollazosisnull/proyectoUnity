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
            if (randomNode != this)
            {
                AddAdjacentNode(randomNode, Random.Range(1f, 5f)); 
            }
        }
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