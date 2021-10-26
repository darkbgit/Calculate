using CalculateVessels.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculateVessels.Core.Models
{
    public class ElementsCollection<T> : IList<T>
    {
        readonly IList<T> _elements;

        public ElementsCollection()
        {
            _elements = new List<T>();
        }

        public ElementsCollection(IList<T> elements)
        {
            _elements = elements;
        }

        public int Count => _elements.Count;

        public bool IsReadOnly => _elements.IsReadOnly;

        public void Add(T item)
        {
            _elements.Add(item);
        }

        public void Clear()
        {
            _elements.Clear();
        }

        public bool Contains(T item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _elements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return _elements.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _elements.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _elements.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _elements.RemoveAt(index);
        }

        public T this[int index]
        {
            get => _elements[index];
            set => _elements[index] = value;
        }
    }
}
