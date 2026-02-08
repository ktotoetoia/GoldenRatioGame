using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserver : IModuleGraphSnapshotObserver, IDisposable
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        private readonly IPortAligner _portAligner = new PortAlignerOrder();
        private readonly Dictionary<IExtensibleModule, IModuleVisualObject> _moduleVisuals = new();
        private readonly ModuleGraphSnapshotDiffer _snapshotDiffer;
        private readonly bool _useInGameObjectPool;
        private readonly Transform _parent;

        public ModuleGraphVisualObserver(Transform parent, bool useInGameObjectPool)
        {
            _parent = parent;
            _useInGameObjectPool = useInGameObjectPool;

            _snapshotDiffer = new ModuleGraphSnapshotDiffer
            {
                OnModuleAdded = HandleModuleAdded,
                OnModuleRemoved = HandleModuleRemoved,
                OnConnected = HandleConnected,
            };
        }

        public void Update()
        {
            if(!_moduleVisuals.Values.Any(x=> x.DirtyTracker.HasDirty)) return;
            
            AlignAll();
            
            foreach (IModuleVisualObject moduleVisualObject in _moduleVisuals.Values)
            {
                moduleVisualObject.DirtyTracker.Clear();
            }
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            _snapshotDiffer.OnGraphUpdated(graph);
        }

        private void HandleModuleAdded(IModule module)
        {
            if (module is not IExtensibleModule extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (_moduleVisuals.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual already exists.");
            if (!extensibleModule.Extensions.TryGetExtension(out IModuleVisual moduleVisual))
                throw new InvalidOperationException("IModuleVisual extension is required.");
            
            HandleModuleAdded(extensibleModule, moduleVisual);
        }

        private void HandleModuleAdded(IExtensibleModule extensibleModule,IModuleVisual moduleVisual)
        {
            IModuleVisualObject visualObject = GetObjectPool(moduleVisual).Get();
            
            _moduleVisuals.Add(extensibleModule, visualObject);
            visualObject.Transform.Transform.SetParent(_parent, false);
            visualObject.Visible = true;
        }

        private void HandleModuleRemoved(IModule module)
        {
            if (module is not IExtensibleModule extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (!_moduleVisuals.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual does not exist.");
            
            HandleModuleRemoved(extensibleModule);
        }

        private void HandleModuleRemoved(IExtensibleModule extensibleModule)
        {
            _moduleVisuals.Remove(extensibleModule, out IModuleVisualObject visualObject);
            GetObjectPool(extensibleModule.Extensions.GetExtension<IModuleVisual>()).Release(visualObject);
        }

        private void HandleConnected(IConnection connection)
        {
            if (connection.Port1.Module is not IExtensibleModule moduleA ||
                connection.Port2.Module is not IExtensibleModule moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!_moduleVisuals.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !_moduleVisuals.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");

            HandleConnected(visualA.GetPortVisualObject(connection.Port1), visualB.GetPortVisualObject(connection.Port2));
        }

        private void HandleConnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            _portAligner.AlignPorts(portA,portB);
        }

        private void AlignAll()
        {
            if (_moduleVisuals.Count == 0 ||
                _moduleVisuals.Keys.FirstOrDefault(x => x is ICoreExtensibleModule) is not ICoreExtensibleModule
                    coreExtensibleModule) return;

            foreach ((IExtensibleModule module, IPort ownerPort) in
                     _traversal.EnumerateModules<IExtensibleModule, IPort>(coreExtensibleModule))
            {
                if (ownerPort == null) continue;

                IPort otherPort = ownerPort.Connection.GetOtherPort(ownerPort);

                if (otherPort?.Module is not IExtensibleModule otherModule)
                    continue;
                if (!_moduleVisuals.TryGetValue(module, out IModuleVisualObject fromModuleVisual))
                    continue;
                if (!_moduleVisuals.TryGetValue(otherModule, out IModuleVisualObject toModuleVisual))
                    continue;

                IPortVisualObject fromVisual = fromModuleVisual.GetPortVisualObject(ownerPort);
                IPortVisualObject toVisual = toModuleVisual.GetPortVisualObject(otherPort);

                if (fromVisual == null || toVisual == null)
                    continue;

                _portAligner.AlignPorts(fromVisual, toVisual);
            }
        }
        
        public void Dispose()
        {
            foreach ((IExtensibleModule module, IModuleVisualObject visualObject) in _moduleVisuals)
            {
                GetObjectPool(module.Extensions.GetExtension<IModuleVisual>()).Release(visualObject as IAnimatedModuleVisualObject);
            }
        }

        private IObjectPool<IModuleVisualObject> GetObjectPool(IModuleVisual moduleVisual)
        {
            return _useInGameObjectPool ? moduleVisual.GamePool : moduleVisual.EditorPool;
        }
    }
}