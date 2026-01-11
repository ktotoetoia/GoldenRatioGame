using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Transforms;

namespace IM.Visuals
{
    public class TransformCommandModuleGraph : ITransformCommandModuleGraph
    {
        private readonly ICommandModuleGraph _commandModuleGraph;
        private readonly TransformGraphStructureBuilder _builder;
        
        public ITransform Transform => _builder.GlobalTransform;
        public IEnumerable<INode> Nodes => _commandModuleGraph.Nodes;
        public IEnumerable<IEdge> Edges => _commandModuleGraph.Edges;
        public IEnumerable<ITransformModule> Modules => _commandModuleGraph.Modules.Cast<ITransformModule>();
        public IEnumerable<ITransformConnection> Connections => _commandModuleGraph.Connections.Cast<ITransformConnection>();
        IEnumerable<IModule> IModuleGraphReadOnly.Modules => _commandModuleGraph.Modules;
        IEnumerable<IConnection> IModuleGraphReadOnly.Connections => _commandModuleGraph.Connections;
        public int CommandsToUndoCount => _commandModuleGraph.CommandsToUndoCount;
        public int CommandsToRedoCount => _commandModuleGraph.CommandsToRedoCount;

        public TransformCommandModuleGraph() : this(new HierarchyTransform())
        {
            
        }

        public TransformCommandModuleGraph(IHierarchyTransform transform)
        {
            _builder = new TransformGraphStructureBuilder(transform);
            _commandModuleGraph = new CommandModuleGraph(new AddModuleAndNotifyObserverCommandFactory(_builder),
                new RemoveAndDisconnectModuleNotifyObserverCommandFactory(_builder),
                new ConnectTransformModulesAndNotifyObserverCommandFactory(_builder),
                new DisconnectModuleAndNotifyObserverCommandFactory(_builder));
        }
        
        public void AddModule(ITransformModule module) => _commandModuleGraph.AddModule(module);
        public void AddAndConnect(ITransformModule module, ITransformPort ownerPort, ITransformPort targetPort) => _commandModuleGraph.AddAndConnect(module, ownerPort, targetPort);
        public void RemoveModule(ITransformModule module) => _commandModuleGraph.RemoveModule(module);
        public ITransformConnection Connect(ITransformPort output, ITransformPort input) => (ITransformConnection)_commandModuleGraph.Connect(output, input);
        public void Disconnect(ITransformConnection connection) => _commandModuleGraph.Disconnect(connection);
        public bool CanUndo(int count) => _commandModuleGraph.CanUndo(count);
        public bool CanRedo(int count) => _commandModuleGraph.CanRedo(count);
        public void Undo(int count) => _commandModuleGraph.Undo(count);
        public void Redo(int count) => _commandModuleGraph.Redo(count);
    }
}