using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Visuals
{
    public class ConnectTransformModulesAndNotifyObserverCommandFactory : IConnectCommandFactory
    {
        private readonly IModuleGraphObserver _observer;
        
        public ConnectTransformModulesAndNotifyObserverCommandFactory(IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }
        
        public IConnectCommand Create(IPort param1, IPort param2, ICollection<IConnection> param3)
        {
            if (param1 is not ITransformPort transformPort1 || param2 is not ITransformPort transformPort2)
                throw new ArgumentException($"{nameof(ConnectTransformModulesAndNotifyObserverCommandFactory)} must be used with transform graph"); 
            
            return new ConnectModulesAndNotifyObserverCommand(new ConnectTransformModulesCommand(transformPort1, transformPort2, param3),_observer);
        }
    }
}