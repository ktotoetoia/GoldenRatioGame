using System.Collections.Generic;
using IM.Base;

namespace IM.Graphs
{
    public class ConnectCommandFactory : IFactory<IConnectCommand, IModulePort, IModulePort, ICollection<IConnection>>
    {
        public IConnectCommand Create(IModulePort param1, IModulePort param2, ICollection<IConnection> param3)
        {
            return new ConnectModulesCommand(param1, param2, param3);
        }
    }
}