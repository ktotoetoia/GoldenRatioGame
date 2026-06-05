namespace IM.Graphs
{
    public interface IValueDataPort<TValue,T> : IDataPort<T>, IHaveNodeValue<TValue>
    {
        
    }
}