using UnityEngine;

public class GraphControlNPC : MonoBehaviour
{
    public NodoControl[] nodesArray;
    public SimpleLinkedList<NodoControl> AllNodes;

    public GameObject player;
    public NPCMovement npc;
    private void Awake()
    {
        AllNodes = new SimpleLinkedList<NodoControl>();
    }

    void Start()
    {
        for (int i = 0; i < nodesArray.Length; i++)
        {
            AllNodes.Add(nodesArray[i]);
        }

        ConnectNodes();
        SetInitialNode();
    }

    void ConnectNodes()
    {
        for (int i = 0; i < AllNodes.Count(); i++)
        {
            NodoControl node = AllNodes.Get(i);
            node.SetRandomAdjacentNodes(AllNodes);

            Debug.Log($"el nodo {node.name} si tiene {node.adjacentNodes.Count()} vecinos");
        }
    }

    void SetInitialNode()
    {
        int position = Random.Range(0, AllNodes.Count());
        NodoControl targetNode = AllNodes.Get(position);

        if (targetNode != null)
        {
            npc.SetInitialNode(targetNode);
            npc.SetNewPosition(targetNode.transform.position);
        }
    }
}