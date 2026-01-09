using System;
using IM.Graphs;

namespace IM.Visuals
{
    public class TransformConnection : ITransformConnection
    {
        public INode Node1 => Output.Module;
        public INode Node2 => Input.Module;

        public ITransformPort Output { get; }
        public ITransformPort Input { get; }

        IPort IConnection.Port1 => Input;
        IPort IConnection.Port2 => Output;

        public TransformConnection(ITransformPort output, ITransformPort input)
        {
            Output = output;
            Input = input;
        }

        public IPort GetOtherPort(IPort port)
        {
            if(Input== port) return Output;
            if(Output == port) return Input;
            
            throw new Exception("Port is not a part of this connection");
        }
    }
}