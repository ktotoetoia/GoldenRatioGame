using System;
using UnityEngine.UIElements;

namespace IM.SaveSystem
{
  [UxmlElement]
    public partial class SaveSlotElement : VisualElement
    {
        private const string UssRoot = "save-slot";
        private const string UssEmpty = "save-slot--empty";
        private const string UssOccupied = "save-slot--occupied";
        private const string UssIndex = "save-slot__index";
        private const string UssInfo = "save-slot__info";
        private const string UssName = "save-slot__name";
        private const string UssMeta = "save-slot__meta";
        private const string UssButton = "save-slot__button";
        private readonly Label _indexLabel;
        private readonly Label _nameLabel;
        private readonly Label _metaLabel;
        private readonly Button _deleteButton;
        private SaveSlotData _data;

        public event Action<int> CreateRequested;
        public event Action<int> LoadRequested;
        public event Action<int> DeleteRequested;
        
        public SaveSlotElement()
        {
            AddToClassList(UssRoot);
            focusable = true;

            _indexLabel = new Label { name = "slot-index" };
            var info = new VisualElement { name = "slot-info" };
            _nameLabel = new Label { name = "slot-name" };
            _metaLabel = new Label { name = "slot-meta" };
 
            _indexLabel.AddToClassList(UssIndex);
            info.AddToClassList(UssInfo);
            _nameLabel.AddToClassList(UssName);
            _metaLabel.AddToClassList(UssMeta);
            
            _deleteButton = new Button()
            {
                text = "Delete",
                name = "slot-delete"
            };
            
            _deleteButton.AddToClassList(UssButton);
            _deleteButton.clicked += () => DeleteRequested?.Invoke(_data.SlotIndex);
            
            info.Add(_nameLabel);
            info.Add(_metaLabel);
 
            Add(_indexLabel);
            Add(info);
            Add(_deleteButton);
            //_deleteButton.
            
            var clickable = new Clickable(OnSlotClicked);
            this.AddManipulator(clickable);
        }
 
        public void Bind(SaveSlotData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            Refresh();
        }
        
        private void Refresh()
        {
            _indexLabel.text = _data.DisplayName;
 
            if (_data.IsEmpty)
            {
                RemoveFromClassList(UssOccupied);
                AddToClassList(UssEmpty);
                _nameLabel.text   = "Empty Slot";
                _metaLabel.text   = string.Empty;
                _deleteButton.enabledSelf =  false;
            }
            else
            {
                RemoveFromClassList(UssEmpty);
                AddToClassList(UssOccupied);
                _nameLabel.text   = $"Playtime: {_data.Playtime}";
                _metaLabel.text   = $"Last saved: {_data.LastSaved}" +
                                    (string.IsNullOrEmpty(_data.ExtraInfo)
                                        ? string.Empty
                                        : $"  •  {_data.ExtraInfo}");
                _deleteButton.enabledSelf =  true;
            }
        }
 
        private void OnSlotClicked()
        {
            if (_data == null) return;
 
            if (_data.IsEmpty)
                CreateRequested?.Invoke(_data.SlotIndex);
            else
                LoadRequested?.Invoke(_data.SlotIndex);
        }
    }
}