using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Augments
{
    public interface IAugmentProgress
    {
        IEntity Entity { get; }
        IEnumerable<AugmentInfo> Augments { get; }
        AugmentProgressInfo Progress { get; }
        
        IEnumerable<AugmentInfo> Advance(int amount);
        IEnumerable<AugmentInfo> GetFinishedAugments();
        IEnumerable<AugmentInfo> GetCurrentAugments();
        IEnumerable<AugmentInfo> GetUnstartedAugments();
    }
}