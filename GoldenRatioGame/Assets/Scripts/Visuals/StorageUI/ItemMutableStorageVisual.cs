using System;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ItemMutableStorageVisual : VisualElement, IStorageVisual
    {
        public ListView ListView { get; private set; }
        public IReadOnlyStorage Storage { get; private set; }

        public ItemMutableStorageVisual()
        {
            ListView = new ListView
            {
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                style = { flexGrow = 1 },
                focusable = false,
                allowAdd = false,
                allowRemove = false,
            };
            
            Add(ListView);
            AddToClassList(StorageClassLists.Storage);
            
            ListView.makeItem = () => new StorageCellUI();
            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellUI cellUI) throw new InvalidOperationException();
                
                cellUI.Cell = Storage[index];
            };
        }

        public void SetStorage(IReadOnlyStorage storage)
        {
            if (storage == null)
            {
                ClearStorage();
                
                return;
            }

            Storage = storage;
            ListView.itemsSource = Storage.GetListForUI();

            storage.ItemAdded += Rebuild;
            storage.ItemRemoved += Rebuild;
            storage.CellsCountChanged += Rebuild;
        }

        public void ClearStorage()
        {
            if(Storage == null) return;
            
            Storage.ItemAdded -= Rebuild;
            Storage.ItemRemoved -= Rebuild;
            Storage.CellsCountChanged -= Rebuild;
            
            Storage = null;
            
            ListView.itemsSource = null;
            ListView.Rebuild();
        }
        
        private void Rebuild(int i, int i1) => ListView.Rebuild();
        private void Rebuild(IStorageCellReadonly storageCellReadonly, IStorableReadOnly storableReadOnly) => ListView.Rebuild();
    }
}