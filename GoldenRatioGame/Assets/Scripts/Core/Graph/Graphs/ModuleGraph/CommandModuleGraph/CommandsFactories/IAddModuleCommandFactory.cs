using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public interface IAddModuleCommandFactory: IFactory<ICommand, IModule, ICollection<IModule>>
    {
        
    }
}