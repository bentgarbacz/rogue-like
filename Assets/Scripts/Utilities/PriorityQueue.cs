using System;
using System.Collections.Generic;

public class PriorityQueue<TElement, TPriority>
{
    private List<(TElement Element, TPriority Priority)> _heap = new();
    private IComparer<TPriority> _comparer;

    public int Count => _heap.Count;

    public PriorityQueue(IComparer<TPriority> comparer = null)
    {
        _comparer = comparer ?? Comparer<TPriority>.Default;
    }

    public void Enqueue(TElement element, TPriority priority)
    {
        _heap.Add((element, priority));
        HeapifyUp(_heap.Count - 1);
    }

    public TElement Dequeue()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        var root = _heap[0].Element;

        // Move the last element to the root and heapify down
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        HeapifyDown(0);

        return root;
    }

    public TElement Peek()
    {
        if (_heap.Count == 0)
            throw new InvalidOperationException("The priority queue is empty.");

        return _heap[0].Element;
    }

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (_comparer.Compare(_heap[i].Priority, _heap[parent].Priority) >= 0)
                break;

            (_heap[i], _heap[parent]) = (_heap[parent], _heap[i]);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        int lastIndex = _heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left <= lastIndex && _comparer.Compare(_heap[left].Priority, _heap[smallest].Priority) < 0)
                smallest = left;

            if (right <= lastIndex && _comparer.Compare(_heap[right].Priority, _heap[smallest].Priority) < 0)
                smallest = right;

            if (smallest == i) break;

            (_heap[i], _heap[smallest]) = (_heap[smallest], _heap[i]);
            i = smallest;
        }
    }
}