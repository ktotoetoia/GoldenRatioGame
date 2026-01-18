using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Storages
{
    public class CellListStorage : ICellListStorage
    {
        private readonly List<IStorageCell> _cells = new();

        public event Action<int, int> CellsCountChanged;
        public event Action<IStorageCell, IStorageItem> ItemAdded;
        public event Action<IStorageCell, IStorageItem> ItemRemoved;

        public int Count => _cells.Count;

        bool ICollection<IStorageCell>.IsReadOnly => false;
        bool IList.IsReadOnly => false;
        public bool IsFixedSize => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;

        int IReadOnlyCollection<IStorageCell>.Count => _cells.Count;
        int ICollection<IStorageCell>.Count => _cells.Count;
        int ICollection.Count => _cells.Count;

        public IStorageCell this[int index]
        {
            get => _cells[index];
            set => _cells[index] = value;
        }

        object IList.this[int index]
        {
            get => _cells[index];
            set => _cells[index] = (IStorageCell)value;
        }

        public IEnumerator<IStorageCell> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IStorageCell item)
        {
            int oldCount = _cells.Count;
            _cells.Add(item);
            CellsCountChanged?.Invoke(_cells.Count, oldCount);
        }

        int IList.Add(object value)
        {
            Add((IStorageCell)value);
            return _cells.Count - 1;
        }

        public void Insert(int index, IStorageCell item)
        {
            int oldCount = _cells.Count;
            _cells.Insert(index, item);
            CellsCountChanged?.Invoke(_cells.Count, oldCount);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (IStorageCell)value);
        }

        public bool Remove(IStorageCell item)
        {
            int oldCount = _cells.Count;
            bool removed = _cells.Remove(item);

            if (removed)
            {
                CellsCountChanged?.Invoke(_cells.Count, oldCount);
            }

            return removed;
        }

        void IList.Remove(object value)
        {
            Remove((IStorageCell)value);
        }

        void IList<IStorageCell>.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            int oldCount = _cells.Count;
            _cells.RemoveAt(index);
            CellsCountChanged?.Invoke(_cells.Count, oldCount);
        }

        void ICollection<IStorageCell>.Clear()
        {
            Clear();
        }

        void IList.Clear()
        {
            Clear();
        }

        public void Clear()
        {
            int oldCount = _cells.Count;
            _cells.Clear();
            CellsCountChanged?.Invoke(_cells.Count, oldCount);
        }

        public bool Contains(IStorageCell item)
        {
            return _cells.Contains(item);
        }

        bool ICollection<IStorageCell>.Contains(IStorageCell item)
        {
            return Contains(item);
        }

        public bool Contains(object value)
        {
            return value is IStorageCell cell && Contains(cell);
        }

        public int IndexOf(IStorageCell item)
        {
            return _cells.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            return value is IStorageCell cell ? IndexOf(cell) : -1;
        }

        public void CopyTo(IStorageCell[] array, int arrayIndex)
        {
            _cells.CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)_cells).CopyTo(array, index);
        }

        bool IReadOnlyStorage.Contains(IStorageCell cell)
        {
            return _cells.Contains(cell);
        }

        int IReadOnlyStorage.IndexOf(IStorageCell cell)
        {
            return _cells.IndexOf(cell);
        }

        public void SetItem(IStorageCell cell, IStorageItem item)
        {
            if (!_cells.Contains(cell))
            {
                throw new ArgumentException($"This storage does not contain cell: {cell}");
            }

            IStorageItem previous = cell.Item;
            cell.Item = item;

            if (previous != null)
            {
                ItemRemoved?.Invoke(cell, previous);
            }

            if (item != null)
            {
                ItemAdded?.Invoke(cell, item);
            }
        }
    }
}