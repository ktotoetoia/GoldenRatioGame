using System;
using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.ModuleGraph
{
    public class ConnectVisualModulesCommand : IConnectCommand
    {
        private readonly IVisualPort _output;
        private readonly IVisualPort _input;
        private readonly ICollection<IConnection> _addTo;
        private bool _isExecuted;

        public IConnection Connection { get; }

        public ConnectVisualModulesCommand(IVisualPort output, IVisualPort input, ICollection<IConnection> addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));

            if (output.Module == input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if (output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");

            Connection = new VisualConnection(output, input);
        }

        public void Execute()
        {
            if (_isExecuted) throw new InvalidOperationException("Command already executed");

            AlignModules();

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

        private void AlignModules()
        {
            Vector3 outputWorldPos = _output.Module.Position + _output.RelativePosition;

            Vector3 outputWorldNormal = _output.Normal.normalized;
            Vector3 inputWorldNormal = _input.Normal.normalized;

            Quaternion rotationToMatch = Quaternion.FromToRotation(inputWorldNormal, -outputWorldNormal);

            RotateModule(_input.Module, rotationToMatch);

            Vector3 rotatedInputPortPos = _input.Module.Position + rotationToMatch * _input.RelativePosition;

            Vector3 offset = outputWorldPos - rotatedInputPortPos;
            _input.Module.Position += offset;
        }

        private void RotateModule(IVisualModule module, Quaternion rotation)
        {
            foreach (IVisualPort port in module.Ports)
            {
                port.RelativePosition = rotation * port.RelativePosition;
                port.Normal = rotation * port.Normal;
            }
        }
    }
}