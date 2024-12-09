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

    void ConnectNodes() // O(N)
    {
        for (int i = 0; i < AllNodes.Count(); i++) // 1 + N (1 + ... + 2)
        {
            NodoControl node = AllNodes.Get(i);  // 1 DE ACCESO + 1 DE ASIGNACION = 2
            node.SetRandomAdjacentNodes(AllNodes);  // 1 DE ACCESO + 1 POR METODO ESPECIAL = 2

            Debug.Log($"el nodo {node.name} si tiene {node.adjacentNodes.Count()} vecinos");  // 1 DE ESCRITURA + 1 DE ACCESO = 2
        }
    }

    void BubbleSortNodesByDistance() //  TIEMPO ASINTOTICO O(N)^2
    {
        for (int i = 0; i < AllNodes.Count() - 1; i++) // 1 + N (1 + ... + 2)
        {
            for (int j = 0; j < AllNodes.Count() - 1 - i; j++)  // 1 + N (1 + ... + 2)
            {
                NodoControl nodeA = AllNodes.Get(j);  // 1 DE ACCESO + 1 DE ASIGNACION = 2
                NodoControl nodeB = AllNodes.Get(j + 1); // 1 DE ACCESO + 1 DE ASIGNACION = 2

                float distanceA = Vector3.Distance(player.transform.position, nodeA.transform.position); // 1 ACCESO + 1 POR METODO = 2
                float distanceB = Vector3.Distance(player.transform.position, nodeB.transform.position);  // 1 ACCESO + 1 POR METODO = 2

                if (distanceA > distanceB) // 1 POR COMPARACION = 1
                {
                    AllNodes.Swap(j, j + 1); // 1 POR METODO ESPECIAL = 1
                }
            }
        }
    }

    void SetInitialNode() //  TIEMPO ASINTOTICO O(N)^2
    {
        BubbleSortNodesByDistance(); // 1 Llamada al método O(N^2)

        NodoControl targetNode = AllNodes.Get(0);  // 1 ACCESO + 1 ASIGNACION = 2
        if (targetNode != null) // 1 COMPARACION = 1
        {
            npc.SetInitialNode(targetNode); // 1 LLAMADA AL METODO = 1
            npc.SetNewPosition(targetNode.transform.position); // 1 LLAMADA AL METODO + 1 ACCESO = 2
        }
    }
}