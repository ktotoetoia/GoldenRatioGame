using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class ConnectModulesCommand : IConnectCommand
    {
        private readonly IModulePort _output;
        private readonly IModulePort _input;
        private readonly ICollection<IConnection> _addTo;
        private bool _isExecuted;
        
        public IConnection Connection { get; }

        public ConnectModulesCommand(IModulePort output, IModulePort input, ICollection<IConnection>  addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            (output, input) = FixPorts(output, input);
            Connection = new Connection(output, input);
            
            if(output.Direction == input.Direction) throw new InvalidOperationException("ports must have different directions");
            if(output.Module == input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if(output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");
        }
        
        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");
            
            _input.Connect(Connection);
            _output.Connect(Connection);
            _addTo.Add(Connection);
            
            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted) throw new InvalidOperationException("Command must be executed before undo");
            
            _input.Disconnect();
            _output.Disconnect();
            _addTo.Remove(Connection);
            
            _isExecuted = false;
        }
        
        private (IModulePort, IModulePort) FixPorts(IModulePort output, IModulePort input)
        {
            return output.Direction == PortDirection.Input ? (input, output) : (output, input);
        }
    }
}