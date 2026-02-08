using IM.Storages;
using UnityEngine.UIElements;

namespace IM.UI
{
    public interface IStorageVisual
    {
        public ListView ListView { get;  }
        public IReadOnlyStorage Storage { get; }
        
        void SetStorage(IReadOnlyStorage storage);
        void ClearStorage();
    }
}