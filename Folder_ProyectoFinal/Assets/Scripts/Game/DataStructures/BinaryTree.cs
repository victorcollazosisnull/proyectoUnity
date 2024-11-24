using System;
using UnityEngine;
class BinaryTree<T>
{
    class Node
    {
        public T Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
        public void AddChild(Node node)
        {
            if (Left == null)
            {
                Left = node;
            }
            else if (Right == null)
            {
                Right = node;
            }
            else
            {
                throw new IndexOutOfRangeException("El sueldo mínimo no te alcanza para mantener más hijos");
            }
        }
    }

    private Node Root { get; set; }
    private int Count { get; set; }
    private DoubleCircularLinkedList<Node> listAllNodes = new DoubleCircularLinkedList<Node>();

    public void AddNode(T value)
    {
        Node newNode = new Node(value);
        if (Root == null)
        {
            Root = newNode;
        }
        else
        {
            if (Root.Left == null)
                Root.Left = newNode;
            else if (Root.Right == null)
                Root.Right = newNode;
            else
                throw new InvalidOperationException("El nodo raíz ya tiene dos hijos.");
        }

        listAllNodes.InsertAtStart(newNode);
        Count++;
    }
    public void PreOrden()
    {
        RecursivePreOrden(Root);
    }
    private void RecursivePreOrden(Node node)
    {
        if (node == null)
        {
            return;
        }
        else
        {
            Console.Write(node.Value + " ");
            RecursivePreOrden(node.Left);
            RecursivePreOrden(node.Right);
        }
    }
    public void DeepSearch()
    {
        PriorityQueue<Node> queueNodes = new PriorityQueue<Node>();
        queueNodes.Enqueue(Root, 0);
    }
}