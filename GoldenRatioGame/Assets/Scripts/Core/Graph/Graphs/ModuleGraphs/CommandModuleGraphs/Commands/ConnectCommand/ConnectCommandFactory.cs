using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConnectCommandFactory : IConnectCommandFactory
    {
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            return new ConnectModulesCommand(param1, param2, param3);
        }
    }
}