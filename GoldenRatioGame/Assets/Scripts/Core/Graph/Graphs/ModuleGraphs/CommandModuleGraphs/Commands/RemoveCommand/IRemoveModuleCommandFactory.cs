using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IRemoveModuleCommandFactory: IFactory<ICommand, IModule, ICollection<IModule>, ICollection<IConnection>>
    {
        
    }
}