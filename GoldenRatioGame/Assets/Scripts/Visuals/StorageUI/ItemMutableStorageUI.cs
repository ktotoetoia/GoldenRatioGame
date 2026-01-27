using System;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ItemMutableStorageUI : VisualElement
    {
        public ListView ListView { get; private set; }
        public IStorage Storage { get; private set; }

        public ItemMutableStorageUI()
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
        }

        public void SetStorage(IStorage storage)
        {
            Storage = storage ?? throw new NullReferenceException("ItemMutableStorage is null");

            ListView.itemsSource = Storage.GetListForUI();
            ListView.makeItem = () => new StorageCellUI();
            
            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellUI cellUI) throw new InvalidOperationException();
                
                cellUI.Cell = Storage[index];
            };

            storage.ItemAdded += (_, _) => ListView.Rebuild();
            storage.ItemRemoved += (_, _) => ListView.Rebuild();
            storage.CellsCountChanged += (_, _) => ListView.Rebuild();
        }
    }
}