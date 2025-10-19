using System;

namespace IM.Graphs
{
    public class Connection : IConnection
    {
        public IPort Input { get; private set; }
        public IPort Output { get; private set; }

        public INode From => Input.Module;
        public INode To => Output.Module;
        
        public Connection(IPort from, IPort to)
        {
            Input = from;
            Output = to;
        }

        public IPort GetOtherPort(IPort port)
        {
            if(Input== port) return Output;
            if(Output == port) return Input;
            
            throw new Exception("Port is not a part of this connection");
        }
    }
}