using IM.LifeCycle;

namespace IM.Graphs
{
    public interface IDataModuleGraphEditor<T, out TDataGraph> : IEditor<TDataGraph, IDataModuleGraphReadOnly<T>>
        where TDataGraph : IDataModuleGraphReadOnly<T>
    {
    }
}