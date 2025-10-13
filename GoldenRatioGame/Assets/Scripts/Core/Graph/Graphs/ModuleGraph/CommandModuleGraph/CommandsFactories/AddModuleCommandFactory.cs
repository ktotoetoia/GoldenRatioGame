using System.Collections.Generic;
using IM.Base;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleCommandFactory : IFactory<ICommand, IModule, ICollection<IModule>>
    {
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            return  new AddModuleCommand(param1, param2);
        }
    }
}