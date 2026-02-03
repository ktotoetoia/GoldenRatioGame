using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using IM.Transforms;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserver : IModuleGraphSnapshotObserver, IModuleGraphStructureUpdater, IDisposable
    {
        private readonly IHierarchyTransform _transform;
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        private readonly IPortAligner _portAligner = new PortAligner();
        private readonly Dictionary<IExtensibleModule, IModuleVisualObject> _moduleVisuals = new();
        private readonly ModuleGraphSnapshotDiffer _snapshotDiffer;

        public ModuleGraphVisualObserver() : this(new HierarchyTransform())
        {
        }

        public ModuleGraphVisualObserver(IHierarchyTransform transform)
        {
            _transform = transform ?? throw new ArgumentNullException(nameof(transform));

            _snapshotDiffer = new ModuleGraphSnapshotDiffer
            {
                OnModuleAdded = HandleModuleAdded,
                OnModuleRemoved = HandleModuleRemoved,
                OnConnected = HandleConnected,
            };
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            _snapshotDiffer.OnGraphUpdated(graph);
        }

        private void HandleModuleAdded(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (module is not IExtensibleModule extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (_moduleVisuals.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual already exists.");
            if (!extensibleModule.Extensions.TryGetExtension(out IModuleVisual moduleVisual))
                throw new InvalidOperationException("IModuleVisual extension is required.");

            IModuleVisualObject visualObject = moduleVisual.GamePool.Get();

            _moduleVisuals.Add(extensibleModule, visualObject);
            _transform.AddChildKeepLocal(visualObject.Transform);
            visualObject.ModuleGraphStructureUpdater = this;
            visualObject.Visibility = true;
        }

        private void HandleModuleRemoved(IModule module)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            if (module is not IExtensibleModule extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (!_moduleVisuals.Remove(extensibleModule, out IModuleVisualObject visualObject))
                throw new InvalidOperationException("Module visual does not exist.");

            _transform.RemoveChild(visualObject.Transform);
            extensibleModule.Extensions.GetExtension<IModuleVisual>().GamePool.Release(visualObject as IAnimatedModuleVisualObject);
        }

        private void HandleConnected(IConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException(nameof(connection));
            if (connection.Port1.Module is not IExtensibleModule moduleA ||
                connection.Port2.Module is not IExtensibleModule moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!_moduleVisuals.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !_moduleVisuals.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");
            
            _portAligner.AlignPorts(
                visualA.GetPortVisualObject(connection.Port1),
                visualB.GetPortVisualObject(connection.Port2));
        }

        public void OnPortTransformChanged(IPort port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));
            
            foreach ((IExtensibleModule module, IPort viaPort) in
                     _traversal.EnumerateModules<IExtensibleModule, IPort>(_moduleVisuals.Keys.FirstOrDefault()))
            {
                IPort otherPort = viaPort.Connection.GetOtherPort(viaPort);

                if (otherPort?.Module is not IExtensibleModule otherModule)
                    continue;
                if (!_moduleVisuals.TryGetValue(module, out IModuleVisualObject fromModuleVisual))
                    continue;
                if (!_moduleVisuals.TryGetValue(otherModule, out IModuleVisualObject toModuleVisual))
                    continue;

                IPortVisualObject fromVisual = fromModuleVisual.GetPortVisualObject(viaPort);
                IPortVisualObject toVisual = toModuleVisual.GetPortVisualObject(otherPort);

                if (fromVisual == null || toVisual == null)
                    continue;

                _portAligner.AlignPorts(fromVisual, toVisual);
            }
        }

        public void Dispose()
        {
            _transform.SetParent(null);
            
            foreach ((IExtensibleModule module, IModuleVisualObject visualObject) in _moduleVisuals)
            {
                module.Extensions.GetExtension<IModuleVisual>().GamePool.Release(visualObject as IAnimatedModuleVisualObject);
            }

            foreach (IHierarchyElement child in _transform.Children)
            {
                _transform.RemoveChild(child);
            }
        }
    }
}