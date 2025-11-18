using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public interface IDisconnectCommandFactory : IFactory<ICommand, IConnection, ICollection<IConnection>>
    {
        
    }
}