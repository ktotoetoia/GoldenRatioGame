using System.Collections;
using System.Linq;
using IM.Storages;
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
                style = { flexGrow = 1 }   
            };

            Add(ListView);
        }

        public void SetStorage(IItemMutableStorage itemMutableStorage)
        {
            ItemMutableStorage = itemMutableStorage ?? throw new System.NullReferenceException("ItemMutableStorage is null");

            ListView.itemsSource = ItemMutableStorage.GetListForUI();
            ListView.makeItem = () => new StorageCellUI();
            
            ListView.bindItem = (element, index) =>
            {
                StorageCellUI label = element as StorageCellUI;
                
                label.Cell = ItemMutableStorage[index];
            };

            itemMutableStorage.ItemAdded += (_, _) => ListView.Rebuild();
            itemMutableStorage.ItemRemoved += (_, _) => ListView.Rebuild();
            itemMutableStorage.CellsCountChanged += (_, _) => ListView.Rebuild();
        }
    }
}