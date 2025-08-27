namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IModule module);
        void RemoveModule(IModule module);
        IModuleConnection Connect(IModulePort from, IModulePort to);
        void Disconnect(IModuleConnection connection);
    }
}