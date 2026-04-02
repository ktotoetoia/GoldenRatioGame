using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public interface ICommandObserverRemoveFactory : IFactory<ICommandObserver, IModule, ICollection<IModule>,ICollection<IConnection>>
    {
        
    }
}