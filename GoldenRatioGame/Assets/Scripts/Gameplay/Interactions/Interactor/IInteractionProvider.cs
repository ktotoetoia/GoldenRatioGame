using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Interactions
{
    public interface IInteractionProvider
    {
        IEnumerable<IInteractable> GetAvailableInteractions(IEntity entity);
    }
}