using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserverBase : IModuleVisualMap, IModuleGraphSnapshotObserver, IDisposable
    {
        private readonly ModuleGraphSnapshotDiffer _snapshotDiffer;
        protected readonly Dictionary<IExtensibleModule, IModuleVisualObject> ModuleVisualObjects = new();
        protected readonly bool UseInGameObjectPool;
        protected readonly ITraversal Traversal;
        protected readonly IPortAligner PortAligner;
        
        public IReadOnlyDictionary<IExtensibleModule, IModuleVisualObject> ModuleToVisualObjects =>  ModuleVisualObjects;
        
        public ModuleGraphVisualObserverBase(bool useInGameObjectPool = false): this(new BreadthFirstTraversal(), new PortAlignerOrder(), useInGameObjectPool)
        {
            
        }
        
        public ModuleGraphVisualObserverBase(ITraversal traversal, IPortAligner portAligner,bool useInGameObjectPool = false)
        {
            UseInGameObjectPool = useInGameObjectPool;
            PortAligner = portAligner;
            Traversal = traversal;

            _snapshotDiffer = new ModuleGraphSnapshotDiffer
            {
                OnModuleAdded = HandleModuleAdded,
                OnModuleRemoved = HandleModuleRemoved,
                OnConnected = HandleConnected,
                OnDisconnected = HandleDisconnected,
            };
        }

        public void Update()
        {
            if(!ModuleVisualObjects.Values.Any(x=> x.DirtyTracker.HasDirty)) return;
            
            AlignAll();
            
            foreach (IModuleVisualObject moduleVisualObject in ModuleVisualObjects.Values)
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
            if (ModuleVisualObjects.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual already exists.");
            if (!extensibleModule.Extensions.TryGetExtension(out IModuleVisual moduleVisual))
                throw new InvalidOperationException("IModuleVisual extension is required.");
            
            HandleModuleAdded(extensibleModule, moduleVisual);
        }

        protected virtual void HandleModuleAdded(IExtensibleModule extensibleModule,IModuleVisual moduleVisual)
        {
            
        }

        private void HandleModuleRemoved(IModule module)
        {
            if (module is not IExtensibleModule extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (!ModuleVisualObjects.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual does not exist.");
            
            HandleModuleRemoved(extensibleModule);
        }

        protected virtual void HandleModuleRemoved(IExtensibleModule extensibleModule)
        {
            
        }

        private void HandleConnected(IConnection connection)
        {
            if (connection.Port1.Module is not IExtensibleModule moduleA ||
                connection.Port2.Module is not IExtensibleModule moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!ModuleVisualObjects.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !ModuleVisualObjects.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");

            HandleConnected(visualA.GetPortVisualObject(connection.Port1), visualB.GetPortVisualObject(connection.Port2));
        }

        protected virtual void HandleConnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            
        }

        private void HandleDisconnected(IConnection connection)
        {
            if (connection.Port1.Module is not IExtensibleModule moduleA ||
                connection.Port2.Module is not IExtensibleModule moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!ModuleVisualObjects.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !ModuleVisualObjects.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");
            
            HandleDisconnected(visualA.GetPortVisualObject(connection.Port1), visualB.GetPortVisualObject(connection.Port2));
        }

        protected virtual void HandleDisconnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            
        }

        private void AlignAll()
        {
            if (ModuleVisualObjects.Count == 0 ||
                ModuleVisualObjects.Keys.FirstOrDefault(x => x is ICoreExtensibleModule) is not ICoreExtensibleModule
                    coreExtensibleModule) return;

            foreach ((IExtensibleModule module, IPort ownerPort) in
                     Traversal.EnumerateModules<IExtensibleModule, IPort>(coreExtensibleModule))
            {
                if (ownerPort == null) continue;

                IPort otherPort = ownerPort.Connection.GetOtherPort(ownerPort);

                if (otherPort?.Module is not IExtensibleModule otherModule) continue;
                if (!ModuleVisualObjects.TryGetValue(module, out IModuleVisualObject fromModuleVisual)) continue;
                if (!ModuleVisualObjects.TryGetValue(otherModule, out IModuleVisualObject toModuleVisual)) continue;

                IPortVisualObject fromVisual = fromModuleVisual.GetPortVisualObject(ownerPort);
                IPortVisualObject toVisual = toModuleVisual.GetPortVisualObject(otherPort);

                if (fromVisual == null || toVisual == null) continue;

                PortAligner.AlignPorts(fromVisual, toVisual);
            }
        }

        protected virtual IObjectPool<IModuleVisualObject> GetObjectPool(IModuleVisual moduleVisual)
        {
            return UseInGameObjectPool ? moduleVisual.GamePool : moduleVisual.EditorPool;
        }
        
        public virtual void Dispose()
        {
            foreach ((IExtensibleModule module, IModuleVisualObject visualObject) in ModuleVisualObjects)
            {
                GetObjectPool(module.Extensions.GetExtension<IModuleVisual>()).Release(visualObject as IAnimatedModuleVisualObject);
            }
        }
    }
}