namespace IM.Graphs
{
    public interface IModuleGraph : IModuleGraphReadOnly
    {
        void AddModule(IModule module);
        void RemoveModule(IModule module);
        IModuleConnection Connect(IModulePort output, IModulePort input);
        void Disconnect(IModuleConnection connection);
    }
}