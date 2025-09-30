using System;
using System.Collections.Generic;

namespace IM.Graphs
{
    public class GameModuleGraph : IGameModuleGraph
    {
        private readonly IModuleGraph _graph;
        private readonly List<IModuleObserver> _observers;
        
        public IReadOnlyList<INode> Nodes => _graph.Nodes;
        public IReadOnlyList<IEdge> Edges => _graph.Edges;
        public IReadOnlyList<IConnection> Connections => _graph.Connections;
        public IReadOnlyList<IModule> Modules => _graph.Modules;
        public IReadOnlyCollection<IModuleObserver> Observers=> _observers;

        public GameModuleGraph() : this(new ModuleGraph())
        {
        }
        
        public GameModuleGraph(IModuleGraph graph) : this(graph, new List<IModuleObserver>())
        {
        }

        public GameModuleGraph(IModuleGraph graph, List<IModuleObserver> observers)
        {
            _graph = graph ?? throw new ArgumentNullException(nameof(graph));
            _observers = observers ?? throw new ArgumentNullException(nameof(observers));
        }
        
        public bool AddModule(IModule module)
        {
            if (!_graph.AddModule(module)) return false;

            NotifyObserversAdd(module);

            return true;
        }

        public bool RemoveModule(IModule module)
        {
            if (!_graph.RemoveModule(module)) return false;
            
            NotifyObserversRemove(module);
            
            return true;
        }

        public IConnection Connect(IModulePort output, IModulePort input)
        {
            return _graph.Connect(output, input);
        }

        public void Disconnect(IConnection connection)
        {
            _graph.Disconnect(connection);
        }

        public void AddObserver(IModuleObserver observer)
        {
            _observers.Add(observer);

            foreach (IModule module in Modules)
            {
                observer.Add(module);
            }
        }

        public void RemoveObserver(IModuleObserver observer)
        {
            _observers.Remove(observer);

            foreach (IModule module in Modules)
            {
                observer.Remove(module);
            }
        }

        public bool AddAndConnect(IModule module, IModulePort modulePort, IModulePort targetPort)
        {
            if(module == null) throw new NullReferenceException(nameof(module));
            if(modulePort == null)  throw new ArgumentNullException(nameof(modulePort));
            if(targetPort == null)  throw new ArgumentNullException(nameof(targetPort));
            
            if (_graph.AddModule(module) && Connect(modulePort, targetPort) != null)
            {
                NotifyObserversAdd(module);

                return true;
            }

            _graph.RemoveModule(module);

            return false;
        }
        
        private void NotifyObserversAdd(IModule module)
        {
            foreach (var observer in _observers)
                observer.Add(module);
        }

        private void NotifyObserversRemove(IModule module)
        {
            foreach (var observer in _observers)
                observer.Remove(module);
        }
    }
}