using System.Collections.Generic;
using IM.Modules;

namespace IM.Augments
{
    public interface IAugmentExtension : IExtension
    {
        IEnumerable<AugmentInfo> Augments { get; }
    }
}