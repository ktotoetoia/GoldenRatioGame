using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Storages
{
    public class CellFactoryStorage : ICellFactoryStorage
    {
        private readonly List<IStorageCell> _cells = new();
        public int Count => _cells.Count;

        public event Action<int, int> CellsCountChanged;
        public event Action<IStorageCell, IStorableReadOnly> ItemAdded;
        public event Action<IStorageCell, IStorableReadOnly> ItemRemoved;
        
        public IStorageCell this[int index] => _cells[index];

        public IStorageCell CreateCell()
        {
            return CreateCellAt(_cells.Count);
        }

        public IStorageCell CreateCellAt(int index)
        {            
            StorageCell cell = new StorageCell();
            
            _cells.Insert(index,cell);
            CellsCountChanged?.Invoke(Count,Count - 1);
            
            return cell;
        }

        public IStorableReadOnly ClearAndRemoveCell(IStorageCell cell)
        {
            IStorableReadOnly stored =ClearCell(cell);
            
            RemoveCell(cell);

            return stored;
        }

        public void RemoveCell(IStorageCell cell)
        {
            if (_cells.Remove(cell))
                CellsCountChanged?.Invoke(Count,Count + 1);
        }

        public IStorableReadOnly ClearCell(IStorageCell cell)
        {            
            if (!_cells.Contains(cell))
            {
                throw new ArgumentException($"This storage does not contains cell: {cell}" );
            }

            IStorableReadOnly item = cell.Item;
            
            cell.Item = null;
            
            ItemRemoved?.Invoke(cell, item);
            
            return item;
        }

        public void SetItem(IStorageCell cell, IStorableReadOnly item)
        {
            if (!_cells.Contains(cell))
            {
                throw new ArgumentException($"This storage does not contain cell: {cell}");
            }
            
            IStorableReadOnly previous = cell.Item;
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

        public bool Contains(IStorageCell cell)
        {
            return _cells.Contains(cell);
        }

        public int IndexOf(IStorageCell cell)
        {
            return _cells.IndexOf(cell);
        }
        
        public IEnumerator<IStorageCell> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IList GetListForUI()
        {
            return _cells;
        }
    }
}