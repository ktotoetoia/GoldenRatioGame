using System.Collections.Generic;
using IM.Common;
using IM.Commands;

namespace IM.Graphs
{
    public interface IRemoveModuleCommandFactory: IFactory<ICommand, IModule, ICollection<IModule>, ICollection<IConnection>>
    {
        
    }
}