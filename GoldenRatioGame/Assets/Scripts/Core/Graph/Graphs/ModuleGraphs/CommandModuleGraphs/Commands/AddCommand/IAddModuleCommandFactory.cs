using System.Collections.Generic;
using IM.Common;
using IM.Commands;

namespace IM.Graphs
{
    public interface IAddModuleCommandFactory: IFactory<ICommand, IModule, ICollection<IModule>>
    {
        
    }
}