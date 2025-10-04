namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IModule module);
        void RemoveModule(IModule module);
        IConnection Connect(IModulePort output, IModulePort input);
        void Disconnect(IConnection connection);
    }
}