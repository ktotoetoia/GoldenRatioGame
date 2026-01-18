using IM.Items;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class ListCell : VisualElement
    {
        private readonly Label _label;
        private IStorageCell _cell;

        public string Name { get => _label.text; set => _label.text = value; }
        
        public IStorageCell Cell
        {
            get => _cell;
            set
            {
                _cell = value;
                _label.text = (value.Item as IHaveName)?.Name ?? "empty";
            }
        }

        public ListCell()
        {
            _label = new Label();

            Add(_label);
            AddToClassList(ListInventoryClassLists.Cell); 
            _label.AddToClassList(ListInventoryClassLists.ItemLabel);
        }
    }
}