using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectCommandFactory : IDisconnectCommandFactory
    {
        public ICommand Create(IConnection param1, ICollection<IConnection> param2)
        {
            return new DisconnectModulesCommand(param1,param2);
        }
    }
}