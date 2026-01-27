using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.Storages
{
    public class CellFactoryStorage : ICellFactoryStorage
    {
        private readonly List<IStorageCell> _cells = new();
        public int Count => _cells.Count;

        public event Action<int, int> CellsCountChanged;
        public event Action<IStorageCellReadonly, IStorableReadOnly> ItemAdded;
        public event Action<IStorageCellReadonly, IStorableReadOnly> ItemRemoved;
        
        public IStorageCellReadonly this[int index] => _cells[index];

        public IStorageCellReadonly CreateCell()
        {
            return CreateCellAt(_cells.Count);
        }

        public IStorageCellReadonly CreateCellAt(int index)
        {            
            StorageCell cell = new StorageCell(this);
            
            _cells.Insert(index,cell);
            CellsCountChanged?.Invoke(Count,Count - 1);
            
            return cell;
        }

        public IStorableReadOnly ClearAndRemoveCell(IStorageCellReadonly cell)
        {
            IStorableReadOnly stored = ClearCell(cell);
            
            RemoveCell(cell);

            return stored;
        }

        public void RemoveCell(IStorageCellReadonly cell)
        {
            if (cell is IStorageCell c && _cells.Remove(c))
                CellsCountChanged?.Invoke(Count,Count + 1);
        }

        public IStorableReadOnly ClearCell(IStorageCellReadonly cell)
        {            
            if (cell is not IStorageCell c || !_cells.Contains(c))
            {
                throw new ArgumentException($"This storage does not contains cell: {cell}" );
            }

            IStorableReadOnly item = cell.Item;
            
            c.SetItem(null);
            
            ItemRemoved?.Invoke(cell, item);
            
            return item;
        }

        public void SetItem(IStorageCellReadonly cell, IStorableReadOnly item)
        {
            if (cell is not IStorageCell c || !_cells.Contains(c))
            {
                throw new ArgumentException($"This storage does not contain cell: {cell}");
            }
            
            IStorableReadOnly previous = cell.Item;
            c.SetItem( item);

            if (previous != null)
            {
                ItemRemoved?.Invoke(cell, previous);
            }
            if (item != null)
            {
                ItemAdded?.Invoke(cell, item);
            }
        }

        public bool Contains(IStorageCellReadonly cell)
        {
            return cell is IStorageCell c && _cells.Contains(c);
        }

        public int IndexOf(IStorageCellReadonly cell)
        {
            if(cell is not IStorageCell c) return -1;
            
            return _cells.IndexOf(c);
        }

        public bool ContainsItem(IStorableReadOnly item)
        {
            return _cells.Any(x => x.Item == item);
        }

        public IStorageCellReadonly GetCell(IStorableReadOnly item)
        {
            return _cells.FirstOrDefault(x => x.Item == item);
        }

        public IEnumerator<IStorageCellReadonly> GetEnumerator()
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