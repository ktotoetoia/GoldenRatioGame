using System.Collections.Generic;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IConnectCommandFactory : IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>>
    {
        
    }
}