using System.Collections.Generic;
using IM.Base;

namespace IM.Graphs
{
    public class ConnectCommandFactory : IFactory<IConnectCommand, IPort, IPort, ICollection<IConnection>>
    {
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            return new ConnectModulesCommand(param1, param2, param3);
        }
    }
}