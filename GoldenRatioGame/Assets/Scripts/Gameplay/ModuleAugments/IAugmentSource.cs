using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Augments
{
    public interface IAugmentSource
    {
        IEnumerable<IAugmentProgress> Progresses { get; }
        
        IAugmentProgress GetFor(IEntity entity);
    }
}