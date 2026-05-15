using System;
using UnityEngine.UIElements;

namespace IM.SaveSystem
{
  [UxmlElement]
    public partial class SaveSlotElement : VisualElement
    {
        public const string UssRoot = "save-slot";
        public const string UssEmpty = "save-slot--empty";
        public const string UssOccupied = "save-slot--occupied";
        public const string UssIndex = "save-slot__index";
        public const string UssInfo = "save-slot__info";
        public const string UssName = "save-slot__name";
        public const string UssMeta = "save-slot__meta";
        
        public event Action<int> OnCreateRequested;
        public event Action<int> OnLoadRequested;
 
        private SaveSlotData _data;
 
        private readonly Label _indexLabel;
        private readonly Label _nameLabel;
        private readonly Label _metaLabel;

        public SaveSlotElement()
        {
            AddToClassList(UssRoot);
            focusable = true;

            _indexLabel = new Label { name = "slot-index" };
            _indexLabel.AddToClassList(UssIndex);
 
            var info = new VisualElement { name = "slot-info" };
            info.AddToClassList(UssInfo);
 
            _nameLabel = new Label { name = "slot-name" };
            _nameLabel.AddToClassList(UssName);
 
            _metaLabel = new Label { name = "slot-meta" };
            _metaLabel.AddToClassList(UssMeta);
 
            info.Add(_nameLabel);
            info.Add(_metaLabel);
 
 
            Add(_indexLabel);
            Add(info);

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
            }
        }
 
        private void OnSlotClicked()
        {
            if (_data == null) return;
 
            if (_data.IsEmpty)
                OnCreateRequested?.Invoke(_data.SlotIndex);
            else
                OnLoadRequested?.Invoke(_data.SlotIndex);
        }
    }
}