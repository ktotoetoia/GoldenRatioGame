using System;
using System.Collections.Generic;
using IM.Commands;
using UnityEngine;

namespace IM.Graphs
{
    public class ConnectModulesCommand : Command, IConnectCommand
    {
        private readonly IPort _output;
        private readonly IPort _input;
        private readonly ICollection<IConnection> _addTo;
        
        public IConnection Connection { get; }

        public ConnectModulesCommand(IPort output, IPort input, ICollection<IConnection>  addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            Connection = new Connection(output, input);
            
            if(output.Module == input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if(output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");
        }

        protected override void InternalExecute()
        {
            _input.Connect(Connection);
            _output.Connect(Connection);
            _addTo.Add(Connection);
        }

        protected override void InternalUndo()
        {
            _input.Disconnect();
            _output.Disconnect();
            _addTo.Remove(Connection);
        }
    }
}