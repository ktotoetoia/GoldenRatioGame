using System.Collections.Generic;

namespace IM.Graphs
{
    public class ModuleGraphDraft : IModuleGraphDraft
    {
        public IReadOnlyList<IConnection> Connections => throw new System.NotImplementedException();
        public IReadOnlyList<IModule> Modules => throw new System.NotImplementedException();
        public IReadOnlyList<INode> Nodes => throw new System.NotImplementedException();
        public IReadOnlyList<IEdge> Edges => throw new System.NotImplementedException();

        public ModuleGraphDraft(IModuleGraph graph)
        {

        }

        public bool AddModule(IModule module)
        {
            throw new System.NotImplementedException();
        }

        public void ApplyChangesToMainGraph()
        {
            throw new System.NotImplementedException();
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            throw new System.NotImplementedException();
        }

        public void Disconnect(IConnection connection)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveModule(IModule module)
        {
            throw new System.NotImplementedException();
        }
    }
}