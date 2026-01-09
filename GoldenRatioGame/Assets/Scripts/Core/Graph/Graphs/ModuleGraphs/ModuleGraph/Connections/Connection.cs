using System;

namespace IM.Graphs
{
    public class Connection : IConnection
    {
        public IPort Port1 { get; private set; }
        public IPort Port2 { get; private set; }

        public INode Node1 => Port1.Module;
        public INode Node2 => Port2.Module;
        
        public Connection(IPort from, IPort to)
        {
            Port1 = from;
            Port2 = to;
        }

        public IPort GetOtherPort(IPort port)
        {
            if(Port1== port) return Port2;
            if(Port2 == port) return Port1;
            
            throw new Exception("Port is not a part of this connection");
        }
    }
}