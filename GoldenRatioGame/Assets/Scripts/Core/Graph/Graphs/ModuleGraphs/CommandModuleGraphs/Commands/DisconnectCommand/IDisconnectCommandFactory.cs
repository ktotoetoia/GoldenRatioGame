using System.Collections.Generic;
using IM.Common;
using IM.Commands;

namespace IM.Graphs
{
    public interface IDisconnectCommandFactory : IFactory<ICommand, IConnection, ICollection<IConnection>>
    {
        
    }
}