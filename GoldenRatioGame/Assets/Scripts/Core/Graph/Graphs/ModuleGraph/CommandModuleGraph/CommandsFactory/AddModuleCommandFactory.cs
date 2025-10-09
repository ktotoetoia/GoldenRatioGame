using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleCommandFactory : IAddModuleCommandFactory
    {
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            return  new AddModuleCommand(param1, param2);
        }
    }
}