using UnityEngine;
using System;
public class DoubleCircularLinkedList<T>
{
    public class Node
    {
        public T Value { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }
        public int Priority { get; set; } 

        public Node(T value)
        {
            Value = value;
            Next = null;
            Previous = null;
            Priority = 0;
        }

        public Node(T value, int priority)
        {
            Value = value;
            Next = null;
            Previous = null;
            Priority = priority;
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
        Count++;
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
            Count++;
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
            throw new IndexOutOfRangeException("Posición fuera de rango.");
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
            Count++;
        }
    }

    public void DeleteAtStart()
    {
        if (Head == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }
        else
        {
            Node lastNode = SearchLastNode();
            Head = Head.Next;
            Head.Previous = lastNode;
            lastNode.Next = Head;
        }
        Count--;
    }

    public void DeleteAtEnd()
    {
        if (Head == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }
        else
        {
            Node lastNode = SearchLastNode();
            Node newNode = lastNode.Previous;
            newNode.Next = Head;
            Head.Previous = newNode;
        }
        Count--;
    }

    public void DeleteAtPosition(int position)
    {
        if (position == 0)
        {
            DeleteAtStart();
        }
        else if (position == Count - 1)
        {
            DeleteAtEnd();
        }
        else if (position >= Count)
        {
            throw new IndexOutOfRangeException("Posición fuera de rango.");
        }
        else
        {
            Node previousNode = Head;
            int iterator = 0;
            while (iterator < position - 1)
            {
                previousNode = previousNode.Next;
                iterator++;
            }
            Node nodeToDelete = previousNode.Next;
            Node nextNode = nodeToDelete.Next;
            previousNode.Next = nextNode;
            nextNode.Previous = previousNode;
            Count--;
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

    public void EnqueueWithPriority(T value, int priority)
    {
        Node newNode = new Node(value, priority);

        if (Head == null)
        {
            Head = newNode;
            newNode.Next = Head;
            newNode.Previous = Head;
        }
        else
        {
            Node currentNode = Head;

            if (newNode.Priority > Head.Priority)
            {
                newNode.Next = Head;
                newNode.Previous = Head.Previous;
                Head.Previous.Next = newNode;
                Head.Previous = newNode;
                Head = newNode;
            }
            else
            {
                while (currentNode.Next != Head && currentNode.Next.Priority >= newNode.Priority)
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

                if (newNode.Next == Head)
                {
                    Head.Previous = newNode;
                }
            }
        }
        Count++;
    }

    public void Dequeue()
    {
        if (Head == null)
        {
            throw new InvalidOperationException("La lista está vacía.");
        }

        if (Head == Head.Next)
        {
            Head = null;
        }
        else
        {
            Node lastNode = Head.Previous;
            Head = Head.Next;
            Head.Previous = lastNode;
            lastNode.Next = Head;
        }

        Count--;
    }
    public void DeleteItemAndShiftLeft(int position)
    {
        if (position < 0 || position >= Count)
        {
            throw new IndexOutOfRangeException("Índice fuera de rango.");
        }

        Node nodeToDelete = GetAtIndex(position);
        Node prevNode = nodeToDelete.Previous;
        Node nextNode = nodeToDelete.Next;

        if (position == 0) 
        {
            Head = nextNode; 
        }

        prevNode.Next = nextNode; 
        nextNode.Previous = prevNode; 

        Count--;

        Node current = nextNode;
        while (current != Head)
        {
            current.Previous = current.Previous.Previous;
            current = current.Next;
        }
    }
}