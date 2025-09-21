namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        bool AddModule(IModule module);
        bool RemoveModule(IModule module);
        IConnection Connect(IModulePort output, IModulePort input);
        void Disconnect(IConnection connection);
    }
}