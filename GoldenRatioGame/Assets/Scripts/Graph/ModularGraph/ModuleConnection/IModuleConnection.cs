namespace IM.Graphs
{
    public interface IModuleConnection : IEdge
    {
        IModulePort Input { get; }
        IModulePort Output { get; }
    }
}