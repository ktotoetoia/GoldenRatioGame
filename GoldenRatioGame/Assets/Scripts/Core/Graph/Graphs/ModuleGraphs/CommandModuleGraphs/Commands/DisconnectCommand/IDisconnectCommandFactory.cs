using System.Collections.Generic;
using IM.Commands;
using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IDisconnectCommandFactory : IFactory<ICommand, IConnection, ICollection<IConnection>>
    {
        
    }
}