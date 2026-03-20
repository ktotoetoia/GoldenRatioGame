using System.Collections.Generic;

namespace IM.Entities
{
    public interface IInteractionProvider
    {
        IEnumerable<IInteractable> GetAvailableInteractions(IEntity entity);
    }
}