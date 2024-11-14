using UnityEngine;
public class SimpleLinkedList<T>
{
    public class Node
    {
        public T Value;
        public Node Next;

        public Node(T value)
        {
            this.Value = value;
            this.Next = null;
        }
    }
    private Node Head;
    private int count;

    public SimpleLinkedList()
    {
        Head = null;
        count = 0;
    }
    public void Add(T value)
    {
        Node newNode = new Node(value);

        if (Head == null)
        {
            Head = newNode;
        }
        else
        {
            Node currentNode = Head;
            while (currentNode.Next != null)
            {
                currentNode = currentNode.Next;
            }
            currentNode.Next = newNode;
        }
        count = count + 1;
    }
    public T Get(int index)
    {
        if (index < 0 || index >= count)
        {
            throw new System.IndexOutOfRangeException("ERROR");
        }
        Node actual = Head;
        for (int i = 0; i < index; i++)
        {
            actual = actual.Next;
        }
        return actual.Value;
    }
    public int Count()
    {
        return count;
    }
}