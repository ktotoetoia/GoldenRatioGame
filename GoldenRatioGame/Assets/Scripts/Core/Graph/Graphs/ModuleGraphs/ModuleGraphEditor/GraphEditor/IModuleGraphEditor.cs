using IM.Common;

namespace IM.Graphs
{
    public interface IModuleGraphEditor<out TModuleGraph> : IEditor<TModuleGraph, IModuleGraphReadOnly>
        where TModuleGraph : IModuleGraphReadOnly 
    {
        
    }
}