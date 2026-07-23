using System.Collections.Generic;

namespace IM.Items
{
    public interface IItemDropObserver
    {
        void OnItemsDropped(IEnumerable<IItem> item);
    }
}