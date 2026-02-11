using System;
using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ItemMutableStorageVisual : VisualElement, IStorageVisual
    {
        public ListView ListView { get; private set; }
        public IReadOnlyStorage Storage { get; private set; }

        public event Action<IStorableReadOnly> ObjectInteracted;
        public event Action<IStorableReadOnly> ObjectSelected;
        public event Action<IStorableReadOnly> ObjectHovered;
        public event Action<IStorableReadOnly> ObjectReleased;
        
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

            ListView.makeItem = () =>
            {
                StorageCellVisual storageCellVisual = new StorageCellVisual();
                
                storageCellVisual.AddManipulator(new ListEntryManipulator(CheckDoubleClick,OnSelected,OnHovered,OnReleased));
                
                return storageCellVisual;
            };
            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellVisual cellUI) throw new InvalidOperationException();
                
                cellUI.Cell = Storage[index];
            };
        }

        private void OnSelected(VisualElement el)
        {
            if (el is not StorageCellVisual cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectSelected?.Invoke(cellVisual.Cell.Item);
        }
        
        private void OnHovered(VisualElement el)
        {
            if (el is not StorageCellVisual cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectHovered?.Invoke(cellVisual.Cell.Item);
        }
        private void OnReleased(VisualElement el)
        {
            if (el is not StorageCellVisual cellVisual || cellVisual.Cell?.Item == null) return;
            ObjectReleased?.Invoke(cellVisual.Cell.Item);
        }
        private void CheckDoubleClick(VisualElement el)
        {
            if (el is not StorageCellVisual cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectInteracted?.Invoke(cellVisual.Cell.Item);
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