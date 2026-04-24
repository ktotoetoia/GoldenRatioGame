using System;
using System.Collections.Generic;
using System.Linq;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class StorageElement : VisualElement, IStorageElement
    {
        private IStorageEvents _events;
        private bool _showEmptyCells = false;

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
            AddToClassList(StorageClassLists.Storage);

            ListView.makeItem = () =>
            {
                var cellUI = new StorageCellElement();
                cellUI.AddManipulator(new ListEntryManipulator(CheckDoubleClick, OnSelected, OnHovered, OnReleased));
                return cellUI;
            };

            ListView.bindItem = (element, index) =>
            {
                if (element is not StorageCellElement cellUI) throw new InvalidOperationException();

                if (ListView.itemsSource[index] is IStorageCellReadonly cell)
                {
                    cellUI.Cell = cell;
                }
            };
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
            if (Storage == null)
            {
                ListView.itemsSource = null;
            }
            else
            {
                ListView.itemsSource = GetFilteredItems().ToList();
            }
            ListView.Rebuild();
        }

        private void OnSelected(VisualElement el) => InvokeIfValid(el, ObjectSelected);
        private void OnHovered(VisualElement el) => InvokeIfValid(el, ObjectHovered);
        private void OnReleased(VisualElement el) => InvokeIfValid(el, ObjectReleased);
        private void CheckDoubleClick(VisualElement el) => InvokeIfValid(el, ObjectInteracted);

        private void InvokeIfValid(VisualElement el, Action<IStorableReadOnly> action)
        {
            if (el is StorageCellElement c && c.Cell.Item != null)
            {
                action?.Invoke(c.Cell.Item);
            }
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
    }
}