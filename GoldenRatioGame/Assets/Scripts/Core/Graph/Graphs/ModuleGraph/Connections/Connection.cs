using System;

namespace IM.Graphs
{
    public class Connection : IConnection
    {
        public IModulePort Input { get; private set; }
        public IModulePort Output { get; private set; }

        public INode From => Input.Module;
        public INode To => Output.Module;
        
        public Connection(IModulePort from, IModulePort to)
        {
            Input = from;
            Output = to;
        }

        public void Connect()
        {
            if (Input == null || Output == null)
                throw new InvalidOperationException("Module connector was disconnected and cannot be reused");
        
            Input.Connect(this);
            Output.Connect(this);
        }

        public void Disconnect()
        {
            if (Input == null || Output == null)
                return;

            if (Input.Connection != this || Output.Connection != this)
                throw new InvalidOperationException("Module connector was changed before disconnecting");
            
            Input.Disconnect();
            Output.Disconnect();
            
            Input = null;
            Output = null;
        }
    }
}