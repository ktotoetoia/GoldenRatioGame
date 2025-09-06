using System;
using System.Collections;
using System.Collections.Generic;

namespace IM.Storages
{
    public class Storage : IStorage
    {
        private readonly List<IStorageCell> _cells = new();
        public int Count => _cells.Count;
        
        public IStorageCell this[int index] => _cells[index];

        public IStorageCell CreateAndInsertCell(int index)
        {
            StorageCell cell = new StorageCell();
            
            _cells.Insert(index,cell);
            
            return cell;
        }
        
        public void SetItem(IStorageCell cell, IStorageItem item)
        {
            if (!_cells.Contains(cell))
            {
                throw new ArgumentException($"This storage does not contains cell: {cell}");
            }
            
            cell.Item = item;
        }

        public IStorageItem EmptyCell(IStorageCell cell)
        {
            if (!_cells.Contains(cell))
            {
                throw new ArgumentException($"This storage does not contains cell: {cell}" );
            }

            IStorageItem item = cell.Item;
            
            cell.Item = null;
            
            return item;
        }
        
        public IEnumerator<IStorageCell> GetEnumerator()
        {
            return _cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}