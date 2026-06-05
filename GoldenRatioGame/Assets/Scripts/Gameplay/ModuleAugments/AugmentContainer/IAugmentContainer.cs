using System.Collections.Generic;

namespace IM.Augments
{
    public interface IAugmentContainer
    {
        IEnumerable<IAugment> Augments { get; }
        
        void Add(IAugment augment);
        bool Remove(IAugment augment);
    }
}