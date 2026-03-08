using System.Collections.Generic;
using System.Linq;
using IM.Commands;
using IM.Graphs;

namespace IM.Modules
{
    public class RemoveModuleDirectObserversCommandFactory : IRemoveModuleCommandFactory
    {
        private readonly List<ICommandObserverRemoveFactory> _directObserverFactories;

        public RemoveModuleDirectObserversCommandFactory(List<ICommandObserverRemoveFactory> directObserverFactories)
        {
            _directObserverFactories = directObserverFactories;
        }

        public ICommand Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            return new ModuleObserverCommand(new RemoveAndDisconnectModuleCommand(param1, param2,param3), _directObserverFactories.Select(x => x.Create(param1,param2,param3)))
            {
                Inverted = true,
            };
        }
    }
}