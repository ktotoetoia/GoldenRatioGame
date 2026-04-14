using System;
using System.Collections.Generic;
using IM.Commands;

namespace IM.Graphs
{
    public class ConnectDataModuleCommand<T> : Command, IDataConnectCommand<T>
    {
        private readonly IDataPort<T> _output;
        private readonly IDataPort<T> _input;
        private readonly ICollection<IDataConnection<T>> _addTo;

        public IDataConnection<T> Connection { get; }

        public ConnectDataModuleCommand(IDataPort<T> output, IDataPort<T> input, ICollection<IDataConnection<T>> addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            Connection = new DataConnection<T>(output,input);
            
            if (output.DataModule == input.DataModule) throw new ArgumentException("Cannot connect ports of the same module.");
            if (output.IsConnected || input.IsConnected) throw new ArgumentException("Port is already connected.");
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