using System;
using IM.Graphs;

namespace IM.Visuals
{
    public class TransformConnection : ITransformConnection
    {
        public INode Node1 => Port2.Module;
        public INode Node2 => Port1.Module;

        public ITransformPort Port2 { get; }
        public ITransformPort Port1 { get; }

        IPort IConnection.Port1 => Port1;
        IPort IConnection.Port2 => Port2;

        public TransformConnection(ITransformPort output, ITransformPort input)
        {
            Port2 = output;
            Port1 = input;
        }

        public IPort GetOtherPort(IPort port)
        {
            if(Port1== port) return Port2;
            if(Port2 == port) return Port1;
            
            throw new Exception("Port is not a part of this connection");
        }
    }
}