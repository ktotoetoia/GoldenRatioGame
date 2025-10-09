using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConnectCommandFactory : IConnectCommandFactory
    {
        public IConnectCommand Create(IModulePort param1, IModulePort param2, ICollection<IConnection> param3)
        {
            return new ConnectModulesCommand(param1, param2, param3);
        }
    }
}