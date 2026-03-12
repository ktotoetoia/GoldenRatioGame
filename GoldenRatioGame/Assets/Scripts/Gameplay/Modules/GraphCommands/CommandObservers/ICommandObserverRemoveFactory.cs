using System.Collections.Generic;
using IM.Common;
using IM.Graphs;

namespace IM.Modules
{
    public interface ICommandObserverRemoveFactory : IFactory<ICommandObserver, IModule, ICollection<IModule>,ICollection<IConnection>>
    {
        
    }
}