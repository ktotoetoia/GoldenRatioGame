using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;

namespace IM.Visuals
{
    public class VisualCommandModuleGraph : IVisualCommandModuleGraph
    {
        private readonly ICommandModuleGraph _commandModuleGraph;
        
        public IHierarchyTransform Transform { get; }
        public IEnumerable<INode> Nodes => _commandModuleGraph.Nodes;
        public IEnumerable<IEdge> Edges => _commandModuleGraph.Edges;
        public IEnumerable<IVisualModule> Modules => _commandModuleGraph.Modules.Cast<IVisualModule>();
        public IEnumerable<IVisualConnection> Connections => _commandModuleGraph.Connections.Cast<IVisualConnection>();
        IEnumerable<IModule> IModuleGraphReadOnly.Modules => _commandModuleGraph.Modules;
        IEnumerable<IConnection> IModuleGraphReadOnly.Connections => _commandModuleGraph.Connections;
        public int CommandsToUndoCount => _commandModuleGraph.CommandsToUndoCount;
        public int CommandsToRedoCount => _commandModuleGraph.CommandsToRedoCount;

        public VisualCommandModuleGraph() : this(new Transform())
        {
            
        }

        public VisualCommandModuleGraph(IHierarchyTransform transform)
        {
            Transform = transform;
            _commandModuleGraph = new CommandModuleGraph(new AddVisualModuleCommandFactory(Transform),
                new RemoveAndDisconnectCommandFactory(), new ConnectVisualCommandFactory(),
                new DisconnectCommandFactory());
        }
        
        public void AddModule(IVisualModule module) => _commandModuleGraph.AddModule(module);
        public void AddAndConnect(IVisualModule module, IVisualPort ownerPort, IVisualPort targetPort) => _commandModuleGraph.AddAndConnect(module, ownerPort, targetPort);
        public void RemoveModule(IVisualModule module) => _commandModuleGraph.RemoveModule(module);
        public IVisualConnection Connect(IVisualPort output, IVisualPort input) => (IVisualConnection)_commandModuleGraph.Connect(output, input);
        public void Disconnect(IVisualConnection connection) => _commandModuleGraph.Disconnect(connection);
        public bool CanUndo(int count) => _commandModuleGraph.CanUndo(count);
        public bool CanRedo(int count) => _commandModuleGraph.CanRedo(count);
        public void Undo(int count) => _commandModuleGraph.Undo(count);
        public void Redo(int count) => _commandModuleGraph.Redo(count);

        public void Dispose()
        {
            foreach (IDisposable module in Modules.OfType<IDisposable>())
            {
                module.Dispose();
            }
        }
    }
}