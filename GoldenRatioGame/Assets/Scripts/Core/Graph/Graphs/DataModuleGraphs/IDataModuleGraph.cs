namespace IM.Graphs
{
    public interface IDataModuleGraph<T> : IDataModuleGraphReadOnly<T>, IDataModuleGraphOperations<T>
    {
    }
}