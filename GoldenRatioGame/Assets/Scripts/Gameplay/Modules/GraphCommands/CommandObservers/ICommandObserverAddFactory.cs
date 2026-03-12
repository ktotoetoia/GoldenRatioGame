using System.Collections.Generic;
using IM.Common;
using IM.Graphs;

namespace IM.Modules
{
    public interface ICommandObserverAddFactory : IFactory<ICommandObserver,IModule, ICollection<IModule>>
    {
        
    }
}