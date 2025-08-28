using System.Linq;
using IM.Graphs;
using IM.SelectionSystem;
using UnityEngine;

namespace IM.ModuleEditor
{
    public class ModuleGraphSelectionProvider : ISelectionProvider
    {
        private readonly IModuleGraph _graph;

        public ModuleGraphSelectionProvider(IModuleGraph graph)
        {
            _graph = graph;
        }

        public ISelection<T> SelectAt<T>(Vector3 position) where T : class
        {
            var matchingPorts = _graph.Modules.SelectMany(x => x.Ports)
                .OfType<IContains>()
                .Where(c => c.Contains(position))
                .OfType<T>();

            if (matchingPorts.Any())
                return new Selection<T>(matchingPorts,position);

            var matchingModules = _graph.Modules
                .OfType<IContains>()
                .Where(c => c.Contains(position))
                .OfType<T>();

            return new Selection<T>(matchingModules,position);
        }

        public ISelection<T> SelectWithin<T>(Bounds bounds) where T : class
        {
            var portsInBounds = _graph.Modules.SelectMany(x => x.Ports)
                .OfType<IHavePosition>()
                .Where(h => bounds.Contains(h.Position))
                .OfType<T>();

            if (portsInBounds.Any())
                return new Selection<T>(portsInBounds,bounds.center);

            var modulesInBounds = _graph.Modules
                .OfType<IHavePosition>()
                .Where(h => bounds.Contains(h.Position))
                .OfType<T>();

            return new Selection<T>(modulesInBounds,bounds.center);
        }
    }
}