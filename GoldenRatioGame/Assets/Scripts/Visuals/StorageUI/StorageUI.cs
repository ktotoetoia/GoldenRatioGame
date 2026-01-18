using System.Collections;
using System.Linq;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageListView : VisualElement
    {
        public ListView ListView { get; private set; }
        public IItemMutableStorage ItemMutableStorage { get; private set; }

        public StorageListView()
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
            if(ItemMutableStorage == null) throw new System.NullReferenceException("ItemMutableStorage is null");

            ItemMutableStorage = itemMutableStorage;

            ListView.itemsSource = ItemMutableStorage as IList ?? ItemMutableStorage.ToList();
            ListView.makeItem = () => new ListCell();
            
            ListView.bindItem = (element, index) =>
            {
                ListCell label = element as ListCell;
                
                label.Cell = ItemMutableStorage[index];
            };

            itemMutableStorage.ItemAdded += (_, _) => ListView.Rebuild();
            itemMutableStorage.ItemRemoved += (_, _) => ListView.Rebuild();
            itemMutableStorage.CellsCountChanged += (_, _) => ListView.Rebuild();
        }
    }
}