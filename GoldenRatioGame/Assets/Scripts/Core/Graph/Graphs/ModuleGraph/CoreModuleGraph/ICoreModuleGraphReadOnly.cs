namespace IM.Graphs
{
    public interface ICoreModuleGraphReadOnly : IModuleGraphReadOnly
    {
        IModule CoreModule { get; }
    }
}