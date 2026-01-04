using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class DisconnectModulesCommand : Command
    {
        private readonly IConnection _connection;
        private readonly ICollection<IConnection> _removeFrom;
    
        public DisconnectModulesCommand(IConnection connection, ICollection<IConnection> removeFrom)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _removeFrom = removeFrom ?? throw new ArgumentNullException(nameof(removeFrom));
        }

        protected override void InternalExecute()
        {
            if (!_connection.Input.IsConnected || !_connection.Output.IsConnected)
                throw new InvalidOperationException("Other command disconnected this connection");

            _connection.Input.Disconnect();
            _connection.Output.Disconnect();
            _removeFrom.Remove(_connection);
        }

        protected override void InternalUndo()
        {
            if (_connection.Input.IsConnected || _connection.Output.IsConnected)
                throw new InvalidOperationException("Other command connected this port");

            _connection.Input.Connect(_connection);
            _connection.Output.Connect(_connection);
            _removeFrom.Add(_connection);
        }
    }
}