using System;

namespace IM.Visuals
{
    public interface IVisualModuleGraph : IVisualModuleGraphReadOnly
    {
        void AddModule(IVisualModule module);
        void AddAndConnect(IVisualModule module, IVisualPort ownerPort, IVisualPort targetPort);
        void RemoveModule(IVisualModule module);
        IVisualConnection Connect(IVisualPort output, IVisualPort input);
        void Disconnect(IVisualConnection connection);
    }
}