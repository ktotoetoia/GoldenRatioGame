using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public interface IRemoveModuleCommandFactory : IFactory<ICommand, IModule, ICollection<IModule>, ICollection<IConnection>>
    {
        
    }
}