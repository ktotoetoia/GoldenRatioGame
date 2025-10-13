using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectCommandFactory : IFactory<ICommand, IConnection, ICollection<IConnection>>
    {
        public ICommand Create(IConnection param1, ICollection<IConnection> param2)
        {
            return new DisconnectModulesCommand(param1,param2);
        }
    }
}