using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectModuleAndNotifyObserverCommand : Command
    {
        private readonly IModuleGraphObserver _observer;
        private readonly IConnection _connection;
        private readonly ICommand _command;
        
        public DisconnectModuleAndNotifyObserverCommand(IConnection connection, ICommand command, IModuleGraphObserver observer)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        protected override void InternalExecute()
        {
            _command.Execute();
            _observer.OnDisconnected(_connection);
        }

        protected override void InternalUndo()
        {
            _command.Undo();
            _observer.OnConnected(_connection);
        }
    }
}