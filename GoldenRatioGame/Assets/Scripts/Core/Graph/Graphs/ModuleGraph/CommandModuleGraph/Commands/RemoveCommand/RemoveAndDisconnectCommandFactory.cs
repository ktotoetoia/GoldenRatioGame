using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectCommandFactory : IRemoveModuleCommandFactory
    {
        public ICommand Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            return new RemoveAndDisconnectModule(param1, param2, param3);
        }
    }
}