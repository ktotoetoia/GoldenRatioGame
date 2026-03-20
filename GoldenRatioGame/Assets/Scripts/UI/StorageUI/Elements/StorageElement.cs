using System;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageElement : VisualElement, IStorageElement
    {
        private IStorageEvents _events;

        public ListView ListView { get; private set; }
        public IReadOnlyStorage Storage { get; private set; }

        public event Action<IStorableReadOnly> ObjectInteracted;
        public event Action<IStorableReadOnly> ObjectSelected;
        public event Action<IStorableReadOnly> ObjectHovered;
        public event Action<IStorableReadOnly> ObjectReleased;
        
        public StorageElement()
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
                StorageCellElement storageCellElement = new StorageCellElement();
                
                storageCellElement.AddManipulator(new ListEntryManipulator(CheckDoubleClick,OnSelected,OnHovered,OnReleased));
                
                return storageCellElement;
            };
            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellElement cellUI) throw new InvalidOperationException();
                
                cellUI.Cell = Storage[index];
            };
        }

        private void OnSelected(VisualElement el)
        {
            if (el is not StorageCellElement cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectSelected?.Invoke(cellVisual.Cell.Item);
        }
        
        private void OnHovered(VisualElement el)
        {
            if (el is not StorageCellElement cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectHovered?.Invoke(cellVisual.Cell.Item);
        }
        private void OnReleased(VisualElement el)
        {
            if (el is not StorageCellElement cellVisual || cellVisual.Cell?.Item == null) return;
            ObjectReleased?.Invoke(cellVisual.Cell.Item);
        }
        private void CheckDoubleClick(VisualElement el)
        {
            if (el is not StorageCellElement cellVisual || cellVisual.Cell?.Item == null) return;
            
            ObjectInteracted?.Invoke(cellVisual.Cell.Item);
        }

        public void SetStorage(IReadOnlyStorage storage, IStorageEvents events)
        {
            if (storage == null)
            {
                ClearStorage();
                
                return;
            }

            Storage = storage;
            _events = events;
            ListView.itemsSource = Storage.GetListForUI();
            
            events.ItemAdded += Rebuild;
            events.ItemRemoved += Rebuild;
            events.CellsCountChanged += Rebuild;
        }

        public void ClearStorage()
        {
            if(Storage == null) return;
            
            _events.ItemAdded -= Rebuild;
            _events.ItemRemoved -= Rebuild;
            _events.CellsCountChanged -= Rebuild;
            
            Storage = null;
            _events = null;
            
            ListView.itemsSource = null;
            ListView.Rebuild();
        }
        
        private void Rebuild(int i, int i1) => ListView.Rebuild();
        private void Rebuild(IStorageCellReadonly storageCellReadonly, IStorableReadOnly storableReadOnly) => ListView.Rebuild();
    }
}