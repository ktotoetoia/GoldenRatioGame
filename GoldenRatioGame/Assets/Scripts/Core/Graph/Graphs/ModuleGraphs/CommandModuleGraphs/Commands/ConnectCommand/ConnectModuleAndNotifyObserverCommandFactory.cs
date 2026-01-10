using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConnectModuleAndNotifyObserverCommandFactory : IConnectCommandFactory
    {
        private readonly IModuleGraphObserver _observer;
        
        public ConnectModuleAndNotifyObserverCommandFactory(IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }
        
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            return new ConnectModulesAndNotifyObserverCommand(new ConnectModulesCommand(param1, param2, param3),_observer);
        }
    }
}