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
        private readonly VisualElement _iconDisplay;
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
            _iconDisplay = new VisualElement();
            
            Add(_iconDisplay);
            Add(_label);
            AddToClassList(StorageClassLists.Cell); 
            _iconDisplay.AddToClassList("icon");
            _label.AddToClassList(StorageClassLists.ItemLabel);
        }
        
        private void UpdateCell(IStorableReadOnly oldStorable, IStorableReadOnly newStorable)
        {
            _label.text = (newStorable as IHaveName)?.Name ?? "empty";
            if (newStorable is IHaveIcon haveIcon)
            {
                _iconDisplay.style.backgroundImage = new StyleBackground(Background.FromSprite(haveIcon.Icon.Sprite) );
            }
        }
    }
}