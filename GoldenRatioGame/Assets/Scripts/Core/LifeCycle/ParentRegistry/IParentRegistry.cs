using System.Collections.Generic;

namespace IM.LifeCycle
{
    public interface IParentRegistry
    {
        IEnumerable<IParentRestorable> TrackedObjects { get; }
        void Register(IParentRestorable target);
        void Unregister(IParentRestorable target);
    }
}