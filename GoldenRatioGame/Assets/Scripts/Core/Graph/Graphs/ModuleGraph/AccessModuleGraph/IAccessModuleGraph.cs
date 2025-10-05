namespace IM.Graphs
{
    public interface IAccessModuleGraph : IModuleGraph
    {
        bool CanUse { get; set; }
    }
}