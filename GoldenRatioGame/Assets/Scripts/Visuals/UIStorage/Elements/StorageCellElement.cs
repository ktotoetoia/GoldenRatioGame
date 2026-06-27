using IM.Items;
using IM.Storages;
using IM.Visuals;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageCellElement : VisualElement, ITooltipInfo
    {
        private readonly Label _label;
        private readonly VisualElement _iconDisplay;
        private IStorageCellReadonly _cell;

        public string Name { get => _label.text; set => _label.text = value; }
        public string ShortDescription => (_cell?.Item as IHaveDescription)?.ShortDescription;
        public string Description => (_cell?.Item as IHaveDescription)?.Description;
        public Sprite Icon => (_cell?.Item as IHaveIcon)?.Icon?.Sprite;
        public object Item => _cell?.Item;

        public IStorageCellReadonly Cell
        {
            get => _cell;
            set
            {
                if (_cell != null) _cell.ItemChanged -= UpdateCell;
                
                _cell = value;
                
                UpdateCell(null,_cell.Item);
            }
        }

        public StorageCellElement()
        {
            _iconDisplay = new VisualElement();
            _label = new Label();
            
            Add(_iconDisplay);
            Add(_label);
            
            AddToClassList(StorageClassLists.Cell); 
            _iconDisplay.AddToClassList(StorageClassLists.Icon);
            _label.AddToClassList(StorageClassLists.ItemLabel);
        }
        
        private void UpdateCell(IStorableReadOnly oldStorable, IStorableReadOnly newStorable)
        {
            _label.text = (newStorable as IHaveName)?.Name ?? "empty";
            if (newStorable is IHaveIcon haveIcon)
            {
                _iconDisplay.style.backgroundImage = new StyleBackground(Background.FromSprite(haveIcon.Icon.Sprite));
            }
        }
    }
}