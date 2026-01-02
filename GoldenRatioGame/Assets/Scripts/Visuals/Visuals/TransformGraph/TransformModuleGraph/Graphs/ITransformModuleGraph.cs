namespace IM.Visuals
{
    public interface ITransformModuleGraph : ITransformModuleGraphReadOnly
    {
        void AddModule(ITransformModule module);
        void AddAndConnect(ITransformModule module, ITransformPort ownerPort, ITransformPort targetPort);
        void RemoveModule(ITransformModule module);
        ITransformConnection Connect(ITransformPort output, ITransformPort input);
        void Disconnect(ITransformConnection connection);
    }
}