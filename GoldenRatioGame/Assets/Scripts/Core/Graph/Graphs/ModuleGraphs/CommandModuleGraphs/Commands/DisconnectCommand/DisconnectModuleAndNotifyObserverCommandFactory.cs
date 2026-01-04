using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectModuleAndNotifyObserverCommandFactory : IDisconnectCommandFactory
    {
        private readonly IModuleGraphObserver _observer;

        public DisconnectModuleAndNotifyObserverCommandFactory(IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }
        
        public ICommand Create(IConnection param1, ICollection<IConnection> param2)
        {
            return new DisconnectModuleAndNotifyObserverCommand(param1, new  DisconnectModulesCommand(param1, param2), _observer);
        }
    }
}