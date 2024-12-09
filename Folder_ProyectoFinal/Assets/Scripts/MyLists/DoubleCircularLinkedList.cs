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

    public void InsertAtStart(T value) //0(1)
    {
        Node newNode = new Node(value); // 1 DE CREACION
        if (Head == null) // 1 DE COMPARACION
        {
            Head = newNode; // 1 DE ASIGNACION
            newNode.Next = Head; // 1 DE ASIGNACION 
            newNode.Previous = Head; // 1 DE ASIGNACION
        }
        else
        {
            Node lastNode = SearchLastNode(); // 1 DE ACCESO + 1 DE LLAMADA A METODO 

            newNode.Next = Head;  // 1 DE ASIGNACION
            newNode.Previous = lastNode;  // 1 DE ASIGNACION
            Head.Previous = newNode;  // 1 DE ASIGNACION
            lastNode.Next = newNode;  // 1 DE ASIGNACION
            Head = newNode;  // 1 DE ASIGNACION
        }
        Count++;  // 1 DE ASIGNACION
    }

    public Node GetAtIndex(int index) //O(1)
    {
        if (index < 0 || index >= Count)  // 1 DE COMPARACION
        {
            throw new IndexOutOfRangeException("Índice fuera de rango."); // 1 DE LECTURA + 1 DE ESCRITURA
        }

        Node currentNode = Head; // 1 DE ASIGNACION
        int i = 0;  // 1 DE ASIGNACION

        while (i < index)  // 1 + N (1 + 2)
        {
            currentNode = currentNode.Next;  // 1 DE ACCESO
            i++; // 1 DE ASIGNACION
        }

        return currentNode; // 1 DE ACCESO
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

    public void InsertAtPosition(T value, int position) // O(N)
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

    public void DeleteAtStart() // O(1)
    {
        if (Head == null) // 1 DE COMPARACION
        {
            throw new InvalidOperationException("La lista está vacía."); // 1 DE LECTURA + 1 DE ESCRITURA     
        }
        else
        {
            Node lastNode = SearchLastNode(); // 1 DE ACCESO + 1 DE LLAMADA A METODO
            Head = Head.Next; // 1 DE ASIGNACION
            Head.Previous = lastNode; // 1 DE ASIGNACION
            lastNode.Next = Head; // 1 DE ASIGNACION
        }
        Count--; // 1 DE ASIGNACION
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

    private Node SearchLastNode() // O(N)
    {
        Node tmp = Head; // 1 DE ASIGNACION
        while (tmp.Next != Head)  // 1 + N (1 + 2)
        {
            tmp = tmp.Next;  // 1 DE ACCESO
        }
        return tmp; // 1 DE ACCESO
    }

    public void EnqueueWithPriority(T value, int priority) // O(N)
    {
        Node newNode = new Node(value, priority); // 1 DE CREACION


        if (Head == null) // 1 DE COMPARACION
        {
            Head = newNode; // 1 DE ASIGNACION
            newNode.Next = Head; // 1 DE ASIGNACION
            newNode.Previous = Head; // 1 DE ASIGNACION
        }
        else
        {
            Node currentNode = Head;  // 1 DE ASIGNACION

            if (newNode.Priority > Head.Priority) // 1 DE COMPARACION
            {
                newNode.Next = Head;  // 1 DE ASIGNACION
                newNode.Previous = Head.Previous;  // 1 DE ASIGNACION
                Head.Previous.Next = newNode; // 1 DE ASIGNACION
                Head.Previous = newNode; // 1 DE ASIGNACION
                Head = newNode; // 1 DE ASIGNACION
            }
            else
            {
                while (currentNode.Next != Head && currentNode.Next.Priority >= newNode.Priority)  // 1 + N (1 + 2)
                {
                    currentNode = currentNode.Next;  // 1 DE ACCESO
                }

                newNode.Next = currentNode.Next; // 1 DE ASIGNACION
                if (currentNode.Next != null) // 1 DE COMPARACION
                {
                    currentNode.Next.Previous = newNode; // 1 DE ASIGNACION
                }
                currentNode.Next = newNode; // 1 DE ASIGNACION
                newNode.Previous = currentNode; // 1 DE ASIGNACION

                if (newNode.Next == Head) // 1 DE ASIGNACION
                {
                    Head.Previous = newNode; // 1 DE ASIGNACION
                }
            }
        }
        Count++; // 1 DE ASIGNACION
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