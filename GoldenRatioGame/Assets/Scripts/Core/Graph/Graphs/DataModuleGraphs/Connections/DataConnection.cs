namespace IM.Graphs
{
    public class DataConnection<T> : IDataConnection<T>
    {
        public IDataPort<T> DataPort1 { get; }
        public IDataPort<T> DataPort2 { get; }
        public INode Node1 => DataPort1.Module;
        public INode Node2  => DataPort2.Module;
        public IPort Port1  => DataPort1;
        public IPort Port2  => DataPort2;
        
        public DataConnection(IDataPort<T> dataPort1, IDataPort<T> dataPort2)
        {
            DataPort1 = dataPort1;
            DataPort2 = dataPort2;
        }
    }
}