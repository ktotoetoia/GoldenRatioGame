using IM.Items;
using IM.Storages;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageCellUI : VisualElement
    {
        private readonly Label _label;
        private IStorageCell _cell;

        public string Name { get => _label.text; set => _label.text = value; }
        
        public IStorageCell Cell
        {
            get => _cell;
            set
            {
                if (_cell != null) _cell.ItemChanged -= UpdateCell;
                
                _cell = value;
                
                UpdateCell(null,_cell.Item);
            }
        }

        public StorageCellUI()
        {
            _label = new Label();

            Add(_label);
            AddToClassList(StorageClassLists.Cell); 
            _label.AddToClassList(StorageClassLists.ItemLabel);
        }

        private void UpdateCell(IStorableReadOnly oldStorable, IStorableReadOnly newStorable)
        {
            _label.text = (newStorable as IHaveName)?.Name ?? "empty";
        }
    }
}