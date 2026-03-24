using System.Collections.Generic;
using IM.LifeCycle;
using IM.Graphs;

namespace IM.Modules
{
    public interface ICommandObserverAddFactory : IFactory<ICommandObserver,IModule, ICollection<IModule>>
    {
        
    }
}