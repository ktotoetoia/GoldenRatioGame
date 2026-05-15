using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace IM.SaveSystem
{
    public class SaveSlotsContainer<T> : VisualElement
    {
        public const string UssRoot = "save-slots-container";

        public event Action<int> OnCreateRequested;
        public event Action<int> OnLoadRequested;
 
        private readonly ISaveSlotFactory<T> _factory;
        private readonly List<SaveSlotElement>  _slotElements = new();
        private readonly VisualElement _container;

        public SaveSlotsContainer(int totalSlots, IReadOnlyList<T> existingSaves, ISaveSlotFactory<T> factory)
        {
            if (totalSlots <= 0) throw new ArgumentOutOfRangeException(nameof(totalSlots), "Must be > 0");
 
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            style.flexGrow = 1;
            _container = new VisualElement();
            
            _container.AddToClassList(UssRoot);
            Add(_container);
 
            BuildSlots(totalSlots, existingSaves ?? Array.Empty<T>());
        }
 
        public void RefreshSlot(int slotIndex, T save)
        {
            if (slotIndex < 0 || slotIndex >= _slotElements.Count)
                throw new ArgumentOutOfRangeException(nameof(slotIndex));
 
            var data = save == null ? _factory.CreateEmpty(slotIndex) : _factory.CreateFromSave(save, slotIndex);
 
            _slotElements[slotIndex].Bind(data);
        }
 
        private void BuildSlots(int totalSlots, IReadOnlyList<T> existingSaves)
        {
            for (int i = 0; i < totalSlots; i++)
            {
                var slot = new SaveSlotElement();
 
                slot.OnCreateRequested += index => OnCreateRequested?.Invoke(index);
                slot.OnLoadRequested   += index => OnLoadRequested?.Invoke(index);
 
                bool hasSave = i < existingSaves.Count && existingSaves[i] != null;
 
                var data = hasSave ? _factory.CreateFromSave(existingSaves[i], i) : _factory.CreateEmpty(i);
 
                slot.Bind(data);
 
                _slotElements.Add(slot);
                _container.Add(slot);
            }
        }
    }
}