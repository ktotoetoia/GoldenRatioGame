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
    }
}