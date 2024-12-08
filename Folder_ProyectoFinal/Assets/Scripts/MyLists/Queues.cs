using System;
using UnityEngine;
class PriorityQueue<T>
{
    private DoubleCircularLinkedList<T> list = new DoubleCircularLinkedList<T>();

    public void Enqueue(T value, int priority)
    {
        list.EnqueueWithPriority(value, priority);
    }

    public void Dequeue()
    {
        list.Dequeue();
    }

    public T Peek()
    {
        if (list.Head == null)
        {
            throw new InvalidOperationException("La cola está vacía.");
        }
        return list.Head.Value;
    }

    public bool IsEmpty()
    {
        return list.Count == 0;
    }

    public int GetCount()
    {
        return list.Count;
    }
}