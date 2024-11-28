using UnityEngine;
using System;
public class DoubleCircularLinkedList<T>
{
    public class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }

        public Node(T value)
        {
            Value = value;
            Next = null;
            Previous = null;
        }
    }
    public Node Head { get; set; }
    public int Count { get; set; }
    public void InsertAtStart(T value)
    {
        Node newNode = new Node(value);
        if (Head == null)
        {
            Head = newNode;
            newNode.Next = Head;
            newNode.Previous = Head;
        }
        else
        {
            Node lastNode = SearchLastNode();
            newNode.Next = Head;
            newNode.Previous = lastNode;
            Head.Previous = newNode;
            lastNode.Next = newNode;
            Head = newNode;
        }
        Count = Count + 1;
    }
    public Node GetAtIndex(int index)
    {
        if (index < 0 || index >= Count) 
        {
            throw new IndexOutOfRangeException("Índice fuera de rango.");
        }

        Node currentNode = Head;
        int i = 0;

        while (i < index)
        {
            currentNode = currentNode.Next;
            i++;
        }

        return currentNode;
    }
    public void InsertAtEnd(T value)
    {
        if (Head == null)
        {
            InsertAtStart(value);
        }
        else
        {
            Node newNode = new Node(value);
            Node lastNode = SearchLastNode();
            newNode.Next = Head;
            newNode.Previous = lastNode;
            lastNode.Next = newNode;
            Head.Previous = newNode;
            Count = Count + 1;
        }
    }
    public void InsertAtPosition(T value, int position)
    {
        if (position == 0)
        {
            InsertAtStart(value);
        }
        else if (position == Count)
        {
            InsertAtEnd(value);
        }
        else if (position > Count)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Node newNode = new Node(value);
            Node previousNode = Head;
            int iterator = 0;
            while (iterator < position - 1)
            {
                previousNode = previousNode.Next;
                iterator++;
            }
            Node nextNode = previousNode.Next;
            previousNode.Next = newNode;
            newNode.Previous = previousNode;
            newNode.Next = nextNode;
            nextNode.Previous = newNode;
            Count = Count + 1;
        }
    }
    public void ModifyAtStart(T value)
    {
        if (Head == null)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Head.Value = value;
        }
    }
    public void ModifyAtEnd(T value)
    {
        if (Head == null)
        {
            ModifyAtStart(value);
        }
        else
        {
            Node lastNode = SearchLastNode();
            lastNode.Value = value;
        }
    }
    public void ModifyAtPosition(T value, int position)
    {
        if (position == 0)
        {
            ModifyAtStart(value);
        }
        else if (position == Count)
        {
            ModifyAtEnd(value);
        }
        else if (position > Count)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Node current = Head;
            int iterator = 0;
            while (iterator < position)
            {
                current = current.Next;
                iterator = iterator + 1;
            }
            current.Value = value;
        }
    }
    public void DeleteAtStart()
    {
        if (Head == null)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Node lastNode = SearchLastNode();
            Head = Head.Next;
            Head.Previous = lastNode;
            lastNode.Next = Head;
        }
        Count = Count - 1;
    }
    public void DeleteAtEnd()
    {
        if (Head == null)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Node lastNode = SearchLastNode();
            Node newNode = lastNode.Previous;
            newNode.Next = Head;
            Head.Previous = newNode;
        }
        Count = Count - 1;
    }
    public void DeleteAtPosition(int position)
    {
        if (position == 0)
        {
            DeleteAtStart();
        }
        else if (position == Count)
        {
            DeleteAtEnd();
        }
        else if (position > Count)
        {
            throw new NullReferenceException("Error");
        }
        else
        {
            Node previousNode = Head;
            int iterator = 0;
            while (iterator < position - 1)
            {
                previousNode = previousNode.Next;
                iterator = iterator + 1;
            }
            Node newNode = previousNode.Next;
            Node nextNode = newNode.Next;
            previousNode.Next = nextNode;
            nextNode.Previous = previousNode;
            Count = Count - 1;
        }
    }
    private Node SearchLastNode()
    {
        Node tmp = Head;
        while (tmp.Next != Head)
        {
            tmp = tmp.Next;
        }
        return tmp;
    }
}