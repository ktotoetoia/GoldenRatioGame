using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IM.Storages
{
    public interface IReadOnlyStorage : IReadOnlyList<IStorageCellReadonly>
    {
        bool Contains(IStorageCellReadonly cell);
        int IndexOf(IStorageCellReadonly cell);
        
        bool ContainsItem(IStorableReadOnly item);
        IStorageCellReadonly GetCell(IStorableReadOnly item);

        IList GetListForUI();
    }

    public class ReadOnlyStorage : IReadOnlyStorage
    {
        private readonly List<IStorageCellReadonly> _storageList = new();
        
        public int Count => _storageList.Count;
        
        public ReadOnlyStorage(IEnumerable<IStorageCellReadonly> storageCells)
        {
            foreach (IStorageCellReadonly storageCell in storageCells)
            {
                IStorageCell cell = new StorageCell(this);
                
                cell.SetItem(storageCell.Item);
                
                _storageList.Add(new StorageCell(this));
            }      
        }
        
        public ReadOnlyStorage()
        {
            
        }

        public IEnumerator<IStorageCellReadonly> GetEnumerator() => _storageList.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_storageList).GetEnumerator();
        public IStorageCellReadonly this[int index] => _storageList[index];
        public bool Contains(IStorageCellReadonly cell) => _storageList.Contains(cell);
        public int IndexOf(IStorageCellReadonly cell) => _storageList.IndexOf(cell);
        public bool ContainsItem(IStorableReadOnly item) => _storageList.Any(x => x.Item == item);
        public IStorageCellReadonly GetCell(IStorableReadOnly item) => _storageList.FirstOrDefault(x => x.Item == item);
        public IList GetListForUI() => _storageList;
    }
}