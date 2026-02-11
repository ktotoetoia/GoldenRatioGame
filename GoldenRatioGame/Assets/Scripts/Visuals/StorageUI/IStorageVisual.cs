using System;
using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    public interface IStorageVisual
    {
        public event Action<IStorableReadOnly> ObjectSelected;
        public event Action<IStorableReadOnly> ObjectHovered;
        public event Action<IStorableReadOnly> ObjectReleased;
        event Action<IStorableReadOnly> ObjectInteracted;
        public ListView ListView { get;  }
        public IReadOnlyStorage Storage { get; }
        
        void SetStorage(IReadOnlyStorage storage);
        void ClearStorage();
    }
}