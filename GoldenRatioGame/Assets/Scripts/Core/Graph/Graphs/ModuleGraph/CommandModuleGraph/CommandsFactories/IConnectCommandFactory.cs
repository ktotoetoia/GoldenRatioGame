using System.Collections.Generic;
using IM.Base;

namespace IM.Graphs
{
    public interface IConnectCommandFactory : IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>>
    {
        
    }
}