using System;
using System.Collections.Generic;
using IM.Commands;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ConnectTransformModulesCommand : Command, IConnectCommand
    {
        private readonly ITransformPort _output;
        private readonly ITransformPort _input;
        private readonly ICollection<IConnection> _addTo;

        public IConnection Connection { get; }

        public ConnectTransformModulesCommand(ITransformPort output, ITransformPort input, ICollection<IConnection> addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            Connection = new TransformConnection(output, input);

            if (output.Module == input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if (output.IsConnected || input.IsConnected)
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