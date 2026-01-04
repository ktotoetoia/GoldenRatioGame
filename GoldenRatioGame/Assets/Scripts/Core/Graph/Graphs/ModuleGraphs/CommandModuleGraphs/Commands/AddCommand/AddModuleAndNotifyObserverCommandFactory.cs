using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class AddModuleAndNotifyObserverCommandFactory : IAddModuleCommandFactory
    {
        private readonly IModuleGraphObserver _observer;
        
        public AddModuleAndNotifyObserverCommandFactory(IModuleGraphObserver observer)
        {
            _observer = observer;
        }
        
        public ICommand Create(IModule param1, ICollection<IModule> param2)
        {
            return  new AddModuleAndNotifyObserverCommand(param1, new AddModuleCommand(param1,param2), _observer);
        }
    }
}