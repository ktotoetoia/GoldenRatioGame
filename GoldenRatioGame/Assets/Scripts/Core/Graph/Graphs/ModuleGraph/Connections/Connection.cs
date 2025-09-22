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
    }
}