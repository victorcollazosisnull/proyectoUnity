using UnityEngine;

public class NodoControl : MonoBehaviour
{
    public SimpleLinkedList<AdjacentNodeInfo> adjacentNodes; 

    private void Awake()
    {
        adjacentNodes = new SimpleLinkedList<AdjacentNodeInfo>();
    }

    public void SetRandomAdjacentNodes(SimpleLinkedList<NodoControl> allNodes) // O(N)
    {
        int numberOfConnections = Random.Range(1, 4);  // 1 operación

        for (int i = 0; i < numberOfConnections; i++) // O(n)
        {
            NodoControl randomNode = allNodes.Get(Random.Range(0, allNodes.Count()));  // O(1) + O(n)

            if (randomNode != this && !IsAlreadyConnected(randomNode))  // 1 + O(n)
            {
                AddAdjacentNode(randomNode, Random.Range(1f, 4f));  // 1 operación
                randomNode.AddAdjacentNode(this, Random.Range(1f, 4f));  // 1 operación
            }
        }

        if (adjacentNodes.Count() == 0 && allNodes.Count() > 1)  // O(1)
        {
            NodoControl fallbackNode = allNodes.Get(Random.Range(0, allNodes.Count()));  // O(1)
            if (fallbackNode != this) // 1 operación
            {
                AddAdjacentNode(fallbackNode, Random.Range(1f, 4f)); // 1 operación
                fallbackNode.AddAdjacentNode(this, Random.Range(1f, 4f)); // 1 operación
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