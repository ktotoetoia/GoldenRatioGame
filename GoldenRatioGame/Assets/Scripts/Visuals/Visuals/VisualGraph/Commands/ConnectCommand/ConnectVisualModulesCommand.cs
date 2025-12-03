using System;
using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class ConnectVisualModulesCommand : IConnectCommand
    {
        private readonly IVisualPort _output;
        private readonly IVisualPort _input;
        private readonly ICollection<IConnection> _addTo;
        private bool _isExecuted;

        public IConnection Connection { get; }

        private Vector3 _prevInputPosition;
        private Quaternion _prevInputRotation;

        public ConnectVisualModulesCommand(IVisualPort output, IVisualPort input, ICollection<IConnection> addTo)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _addTo = addTo ?? throw new ArgumentNullException(nameof(addTo));
            Connection = new VisualConnection(output, input);

            if (output.Module == input.Module)
                throw new ArgumentException("Cannot connect ports of the same module.");
            if (output.IsConnected || input.IsConnected)
                throw new ArgumentException("Port is already connected.");
        }

        public void Execute()
        {
            if (_isExecuted)
                throw new InvalidOperationException("Command already executed");

            AlignModules();

            _input.Connect(Connection);
            _output.Connect(Connection);
            _addTo.Add(Connection);

            _isExecuted = true;
        }

        public void Undo()
        {
            if (!_isExecuted)
                throw new InvalidOperationException("Command must be executed before undo");

            ITransform inputTransform = _input.Module.Transform;
            inputTransform.LocalPosition = _prevInputPosition;
            inputTransform.LocalRotation = _prevInputRotation;

            _input.Disconnect();
            _output.Disconnect();
            _addTo.Remove(Connection);

            _isExecuted = false;
        }

        private void AlignModules()
        {
            ITransform outputT = _output.Module.Transform;
            ITransform inputT  = _input.Module.Transform;

            _prevInputPosition = inputT.Position;
            _prevInputRotation = inputT.Rotation;

            Vector3 outputLocal = Vector3.Scale(_output.Transform.LocalPosition, outputT.LossyScale);
            Vector3 inputLocal  = Vector3.Scale(_input.Transform.LocalPosition, inputT.LossyScale);

            Vector3 outputWorldPos = outputT.Position + outputT.Rotation * outputLocal;

            Quaternion outputWorldRot = outputT.Rotation * _output.Transform.LocalRotation;
            Quaternion inputWorldRot  = inputT.Rotation * _input.Transform.LocalRotation;

            Quaternion deltaRot =
                Quaternion.FromToRotation(inputWorldRot * Vector3.forward,
                    -(outputWorldRot * Vector3.forward));

            Quaternion desiredRotation = deltaRot * inputT.Rotation;

            Vector3 rotatedInputRelPos = desiredRotation * inputLocal;

            Vector3 desiredPosition = outputWorldPos - rotatedInputRelPos;

            inputT.Position = desiredPosition;
            inputT.Rotation = desiredRotation;
        }
    }
}