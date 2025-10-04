using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class DisconnectModulesCommand : ICommand
    {
        private readonly IConnection _connection;
        private readonly ICollection<IConnection> _removeFrom;
        private bool _isExecuted;
    
        public DisconnectModulesCommand(IConnection connection, ICollection<IConnection> removeFrom)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _removeFrom = removeFrom ?? throw new ArgumentNullException(nameof(removeFrom));
        }
    
        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");
            if (!_connection.Input.IsConnected || !_connection.Output.IsConnected)
                throw new InvalidOperationException("Other command disconnected this connection");

            _connection.Input.Disconnect();
            _connection.Output.Disconnect();
            _removeFrom.Remove(_connection);

            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            if (_connection.Input.IsConnected || _connection.Output.IsConnected)
                throw new InvalidOperationException("Other command connected this port");

            _connection.Input.Connect(_connection);
            _connection.Output.Connect(_connection);
            _removeFrom.Add(_connection);

            _isExecuted = false;
        }
    }
}