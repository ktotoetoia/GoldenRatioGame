using System;
using IM.Graphs;

namespace IM.Visuals
{
    public class VisualConnection : IVisualConnection
    {
        public INode From => Output.Module;
        public INode To => Input.Module;

        public IVisualPort Output { get; }
        public IVisualPort Input { get; }

        IPort IConnection.Input => Input;
        IPort IConnection.Output => Output;

        public VisualConnection(IVisualPort output, IVisualPort input)
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