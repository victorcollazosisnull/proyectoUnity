using System;
using UnityEngine;
class PriorityQueue<T>
{
    class Node
    {
        public T Value { get; set; }
        public int Priority { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public Node(T value, int priority)
        {
            Value = value;
            Priority = priority;
            Next = null;
            Previous = null;
        }
    }
    private Node Head { get; set; }
    private Node Tail { get; set; }
    private int Count { get; set; }

    public void Enqueue(T value, int priority)
    {
        Node newNode = new Node(value, priority);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            Node currentNode = Head;
            if (newNode.Priority < Head.Priority)
            {
                newNode.Next = Head;
                Head.Previous = newNode;
                Head = newNode;
            }
            else
            {
                while (currentNode.Next != null && currentNode.Next.Priority <= newNode.Priority)
                {
                    currentNode = currentNode.Next;
                }
                newNode.Next = currentNode.Next;
                if (currentNode.Next != null)
                {
                    currentNode.Next.Previous = newNode;
                }
                currentNode.Next = newNode;
                newNode.Previous = currentNode;
                if (newNode.Next == null)
                {
                    Tail = newNode;
                }
            }
        }
        Count = Count + 1;
    }
    public void Dequeue()
    {
        if (Head == null)
        {
            throw new InvalidOperationException("La cola está vacía.");
        }
        else if (Head == Tail)
        {
            Head = null;
            Tail = null;
        }
        else
        {
            Node newHead = Head.Next;
            Head.Next = null; 
            newHead.Previous = null; 
            Head = newHead; 
        }
        Count = Count - 1;
    }
}