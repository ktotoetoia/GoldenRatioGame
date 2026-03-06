using System.Collections.Generic;
using IM.Common;

namespace IM.Graphs
{
    public interface IConnectCommandFactory : IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>>
    {
        
    }
}