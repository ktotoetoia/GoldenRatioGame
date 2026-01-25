using System;
using System.Collections;
using System.Linq;
using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ItemMutableStorageUI : VisualElement
    {
        public ListView ListView { get; private set; }
        public IItemMutableStorage ItemMutableStorage { get; private set; }

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

        public void SetStorage(IItemMutableStorage itemMutableStorage)
        {
            ItemMutableStorage = itemMutableStorage ?? throw new NullReferenceException("ItemMutableStorage is null");

            ListView.itemsSource = ItemMutableStorage.GetListForUI();
            ListView.makeItem = () => new StorageCellUI();
            
            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellUI cellUI) throw new InvalidOperationException();
                
                cellUI.Cell = ItemMutableStorage[index];
            };

            itemMutableStorage.ItemAdded += (_, _) => ListView.Rebuild();
            itemMutableStorage.ItemRemoved += (_, _) => ListView.Rebuild();
            itemMutableStorage.CellsCountChanged += (_, _) => ListView.Rebuild();
        }
    }
}