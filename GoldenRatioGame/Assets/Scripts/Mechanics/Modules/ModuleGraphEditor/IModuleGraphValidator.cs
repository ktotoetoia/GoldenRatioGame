using IM.Graphs;

namespace IM.Modules
{
    public interface IModuleGraphValidator
    {
        bool IsValid(IModuleGraphReadOnly graph);
    }
}