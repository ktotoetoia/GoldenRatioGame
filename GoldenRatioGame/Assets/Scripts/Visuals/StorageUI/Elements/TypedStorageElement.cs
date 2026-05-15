using System;
using System.Collections.Generic;
using System.Linq;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    [UxmlElement]
    public partial class TypedStorageElement : StorageElement
    {
        private Type _targetType;

        public Type TargetType
        {
            get => _targetType;
            set
            {
                if (_targetType == value) return;
                _targetType = value;
                RefreshListViewSource();
            }
        }

        public TypedStorageElement()
        {
            ShowEmptyCells = false;
        }

        protected override IEnumerable<IStorageCellReadonly> GetFilteredItems()
        {
            var baseItems = base.GetFilteredItems();

            return TargetType == null ? baseItems : baseItems.Where(cell => cell.Item != null && TargetType.IsInstanceOfType(cell.Item));
        }
    }
}