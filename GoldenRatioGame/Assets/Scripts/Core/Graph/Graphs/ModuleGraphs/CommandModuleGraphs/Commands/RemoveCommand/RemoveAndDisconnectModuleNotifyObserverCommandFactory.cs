using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class RemoveAndDisconnectModuleNotifyObserverCommandFactory : IRemoveModuleCommandFactory
    {
        private readonly IModuleGraphObserver _observer;
        
        public RemoveAndDisconnectModuleNotifyObserverCommandFactory(IModuleGraphObserver observer)
        {
            _observer = observer;
        }
        
        public ICommand Create(IModule param1, ICollection<IModule> param2, ICollection<IConnection> param3)
        {
            return new RemoveAndDisconnectModuleNotifyObserverCommand(param1, param2, param3, _observer);
        }
    }
}