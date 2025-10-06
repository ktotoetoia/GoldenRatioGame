namespace IM.Graphs
{
    public interface IModuleGraphValidator
    {
        bool IsValid(IModuleGraphReadOnly graph);
        bool TryFix(IModuleGraph graph);
    }
}