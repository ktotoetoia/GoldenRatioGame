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
            if (!_connection.Port1.IsConnected || !_connection.Port2.IsConnected)
                throw new InvalidOperationException("Other command disconnected this connection");

            _connection.Port1.Disconnect();
            _connection.Port2.Disconnect();
            _removeFrom.Remove(_connection);
        }

        protected override void InternalUndo()
        {
            if (_connection.Port1.IsConnected || _connection.Port2.IsConnected)
                throw new InvalidOperationException("Other command connected this port");

            _connection.Port1.Connect(_connection);
            _connection.Port2.Connect(_connection);
            _removeFrom.Add(_connection);
        }
    }
}