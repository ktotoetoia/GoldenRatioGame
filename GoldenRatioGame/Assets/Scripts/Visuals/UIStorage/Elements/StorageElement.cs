using System;
using System.Collections.Generic;
using System.Linq;
using IM.Items;
using IM.Storages;
using IM.Visuals;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageElement : VisualElement, IStorageElement
    {
        private IStorageEvents _events;
        private bool _showEmptyCells = false;
 
        public const string StorageClass = "storage";
        public const string CellClass    = "storage__cell";
        
        [UxmlAttribute]
        public bool ShowEmptyCells
        {
            get => _showEmptyCells;
            set
            {
                if (_showEmptyCells == value) return;
                _showEmptyCells = value;
                RefreshListViewSource();
            }
        }
 
        public ListView ListView { get; private set; }
        public IReadOnlyStorage Storage { get; private set; }
 
        public event Action<IStorableReadOnly> ObjectInteracted;
        public event Action<IStorableReadOnly> ObjectSelected;
        public event Action<IStorableReadOnly> ObjectHovered;
        public event Action<IStorableReadOnly> ObjectReleased;
 
        public StorageElement()
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
            AddToClassList(StorageClass);
 
            ListView.makeItem = MakeItem;
            ListView.bindItem = BindItem;
            ListView.unbindItem = UnbindItem;

            var a = new CellFactoryStorage();
            
            SetStorage(a,a);

            a.SetItemToFirstOrNew(new C());
            a.SetItemToFirstOrNew(new C());
            a.SetItemToFirstOrNew(new C());
            a.SetItemToFirstOrNew(new C());
            a.SetItemToFirstOrNew(new C());
            a.SetItemToFirstOrNew(new C());
        }

        private class C : IItem, IStorable
        {
            public string Name => "Name";
            public string ShortDescription => "ShortDescription";
            public string Description=> "Description";
            public IIcon Icon => new Icon(Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f));
            public object Owner => this;
            public bool SetOwner(object owner)
            {
                return true;
            }

            public IStorageCell Cell { get; set; }
        }
 
        private VisualElement MakeItem()
        {
            var itemInfoElement = new ItemInfoElement();
            itemInfoElement.AddToClassList(CellClass);
            itemInfoElement.AddManipulator(new ListEntryManipulator(CheckDoubleClick, OnSelected, OnHovered, OnReleased));
 
            itemInfoElement.userData = new CellBinding(itemInfoElement);
 
            return itemInfoElement;
        }
 
        private void BindItem(VisualElement element, int index)
        {
            if (element is not ItemInfoElement itemInfoElement)
                throw new InvalidOperationException();
 
            if (ListView.itemsSource[index] is not IStorageCellReadonly cell)
                return;
 
            ((CellBinding)itemInfoElement.userData).Bind(cell);
        }
 
        private void UnbindItem(VisualElement element, int index)
        {
            if (element is ItemInfoElement itemInfoElement)
                ((CellBinding)itemInfoElement.userData)?.Unbind();
        }
 
        protected virtual IEnumerable<IStorageCellReadonly> GetFilteredItems()
        {
            if (Storage == null) return Enumerable.Empty<IStorageCellReadonly>();
 
            return ShowEmptyCells
                ? Storage
                : Storage.Where(cell => cell.Item != null);
        }
 
        public void RefreshListViewSource()
        {
            ListView.itemsSource = Storage == null ? null : GetFilteredItems().ToList();
            ListView.Rebuild();
        }
 
        private void OnSelected(VisualElement el) => InvokeIfValid(el, ObjectSelected);
        private void OnHovered(VisualElement el) => InvokeIfValid(el, ObjectHovered);
        private void OnReleased(VisualElement el) => InvokeIfValid(el, ObjectReleased);
        private void CheckDoubleClick(VisualElement el) => InvokeIfValid(el, ObjectInteracted);
 
        private void InvokeIfValid(VisualElement el, Action<IStorableReadOnly> action)
        {
            if (el is ItemInfoElement { Item: IStorableReadOnly item })
                action?.Invoke(item);
        }
 
        public void SetStorage(IReadOnlyStorage storage, IStorageEvents events)
        {
            if (storage == null)
            {
                ClearStorage();
                return;
            }
 
            Storage = storage;
            _events = events;
 
            _events.ItemAdded += Rebuild;
            _events.ItemRemoved += Rebuild;
            _events.CellsCountChanged += Rebuild;
 
            RefreshListViewSource();
        }
 
        public void ClearStorage()
        {
            if (Storage == null) return;
 
            _events.ItemAdded -= Rebuild;
            _events.ItemRemoved -= Rebuild;
            _events.CellsCountChanged -= Rebuild;
 
            Storage = null;
            _events = null;
 
            ListView.itemsSource = null;
            ListView.Rebuild();
        }
 
        private void Rebuild(int i, int i1) => RefreshListViewSource();
        private void Rebuild(IStorageCellReadonly cell, IStorableReadOnly item) => RefreshListViewSource();
 
        private sealed class CellBinding
        {
            private readonly ItemInfoElement _element;
            private IStorageCellReadonly _cell;
 
            public CellBinding(ItemInfoElement element) => _element = element;
 
            public void Bind(IStorageCellReadonly cell)
            {
                Unbind();
 
                _cell = cell;
                _cell.ItemChanged += OnItemChanged;
                _element.SetItem(cell.Item);
            }
 
            public void Unbind()
            {
                if (_cell == null) return;
 
                _cell.ItemChanged -= OnItemChanged;
                _cell = null;
            }
 
            private void OnItemChanged(IStorableReadOnly oldItem, IStorableReadOnly newItem) =>
                _element.SetItem(newItem);
        }
    }
}