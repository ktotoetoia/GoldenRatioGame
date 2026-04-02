using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IAddModuleCommandFactory: IFactory<ICommand, IModule, ICollection<IModule>>
    {
        
    }
}