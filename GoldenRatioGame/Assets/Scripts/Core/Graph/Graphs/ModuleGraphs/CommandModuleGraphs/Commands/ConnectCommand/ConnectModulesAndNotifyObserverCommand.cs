using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class ConnectModulesAndNotifyObserverCommand : Command, IConnectCommand
    {
        private readonly IModuleGraphObserver _observer;
        private readonly IConnectCommand _command;
        
        public IConnection Connection => _command.Connection;
        
        public ConnectModulesAndNotifyObserverCommand(IConnectCommand command, IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        protected override void InternalExecute()
        {
            _command.Execute();
            _observer.OnConnected(Connection);
        }

        protected override void InternalUndo()
        {
            _command.Undo();
            _observer.OnDisconnected(Connection);
        }

    }
}