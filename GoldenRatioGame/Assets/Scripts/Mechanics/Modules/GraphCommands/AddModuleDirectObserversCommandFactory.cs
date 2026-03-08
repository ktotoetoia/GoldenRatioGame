using System.Collections.Generic;
using System.Linq;
using IM.Commands;
using IM.Graphs;

namespace IM.Modules
{
    public class AddModuleDirectObserversCommandFactory : IAddModuleCommandFactory
    {
        private readonly List<ICommandObserverAddFactory> _directObserverFactories;

        public AddModuleDirectObserversCommandFactory(List<ICommandObserverAddFactory> directObserverFactories)
        {
            _directObserverFactories = directObserverFactories;
        }

        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            return new ModuleObserverCommand(new AddModuleCommand(param1, param2), _directObserverFactories.Select(x => x.Create(param1,param2)));
        }
    }
}