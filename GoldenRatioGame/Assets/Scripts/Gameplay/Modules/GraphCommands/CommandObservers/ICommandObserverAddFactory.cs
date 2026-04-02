using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Modules
{
    public interface ICommandObserverAddFactory : IFactory<ICommandObserver,IModule, ICollection<IModule>>
    {
        
    }
}