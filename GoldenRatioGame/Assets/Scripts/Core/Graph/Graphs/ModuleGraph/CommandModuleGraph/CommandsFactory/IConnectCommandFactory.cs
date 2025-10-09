using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public interface IConnectCommandFactory : IFactory<IConnectCommand, IModulePort, IModulePort, ICollection<IConnection>>
    {
        
    }
}