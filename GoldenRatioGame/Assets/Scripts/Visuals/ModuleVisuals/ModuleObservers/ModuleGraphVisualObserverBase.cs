using System;
using System.Collections.Generic;
using System.Linq;
using IM.Graphs;
using IM.Modules;
using UnityEngine.Pool;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserverBase : IModuleVisualMap, IDisposable, IEditorObserver<IModuleEditingContextReadOnly>
    {
        private readonly ModuleGraphSnapshotDiffer _snapshotDiffer;
        protected readonly Dictionary<IDataModule<IExtensibleItem>, IModuleVisualObject> ModuleVisualObjects = new();
        protected readonly Dictionary<IDataPort<IExtensibleItem>, IPortVisualObject> PortsVisualObjects = new();
        protected readonly bool UseInGameObjectPool;
        protected readonly ITraversal Traversal;
        protected readonly IPortAligner PortAligner;
        
        public IReadOnlyDictionary<IDataModule<IExtensibleItem>, IModuleVisualObject> ModuleToVisualObjects =>  ModuleVisualObjects;
        public IReadOnlyDictionary<IDataPort<IExtensibleItem>, IPortVisualObject> PortToVisualObjects => PortsVisualObjects;
        
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
                ModuleAdded = HandleModuleAdded,
                ModuleRemoved = HandleModuleRemoved,
                Connected = HandleConnected,
                Disconnected = HandleDisconnected,
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

        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            if (snapshot == null) throw new ArgumentNullException(nameof(snapshot));

            _snapshotDiffer.OnSnapshotChanged(snapshot.Graph);
        }

        private void HandleModuleAdded(IModule module)
        {
            if (module is not IDataModule<IExtensibleItem> extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (ModuleVisualObjects.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual already exists.");
            if (!extensibleModule.Value.Extensions.TryGet(out IModuleVisualObjectProvider moduleVisual))
                throw new InvalidOperationException("IModuleVisual extension is required.");
            
            HandleModuleAdded(extensibleModule, moduleVisual);
        }
        
        private void HandleModuleRemoved(IModule module)
        {
            if (module is not IDataModule<IExtensibleItem> extensibleModule)
                throw new InvalidOperationException("Module must implement IExtensibleModule.");
            if (!ModuleVisualObjects.ContainsKey(extensibleModule))
                throw new InvalidOperationException("Module visual does not exist.");
            
            HandleModuleRemoved(extensibleModule);
        }
        
        private void HandleConnected(IConnection connection)
        {
            if (connection.Port1.Module is not IDataModule<IExtensibleItem> moduleA ||
                connection.Port2.Module is not IDataModule<IExtensibleItem> moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!ModuleVisualObjects.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !ModuleVisualObjects.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");

            var a = visualA.PortsVisualObjects[moduleA.Ports.ToList().IndexOf(connection.Port1)];
            var b = visualB.PortsVisualObjects[moduleB.Ports.ToList().IndexOf(connection.Port2)];
            
            HandleConnected(a,b);
        }

        private void HandleDisconnected(IConnection connection)
        {
            if (connection.Port1.Module is not IDataModule<IExtensibleItem> moduleA ||
                connection.Port2.Module is not IDataModule<IExtensibleItem> moduleB)
                throw new InvalidOperationException("Both modules must implement IExtensibleModule.");
            if (!ModuleVisualObjects.TryGetValue(moduleA, out IModuleVisualObject visualA) ||
                !ModuleVisualObjects.TryGetValue(moduleB, out IModuleVisualObject visualB))
                throw new InvalidOperationException("Module visual does not exist.");
            
            var a = visualA.PortsVisualObjects[moduleA.Ports.ToList().IndexOf(connection.Port1)];
            var b = visualB.PortsVisualObjects[moduleB.Ports.ToList().IndexOf(connection.Port2)];
            
            HandleDisconnected(a,b);
        }
        
        private void AlignAll()
        {
            if (ModuleVisualObjects.Count == 0 ||
                ModuleVisualObjects.Keys.FirstOrDefault(x => x.Value is ICoreExtensibleItem) is not
                    { } coreExtensibleModule) return;

            foreach ((IDataModule<IExtensibleItem> module, IPort ownerPort) in
                     Traversal.EnumerateModules<IDataModule<IExtensibleItem>, IPort>(coreExtensibleModule))
            {
                if (ownerPort == null) continue;

                IPort otherPort = ownerPort.Connection.GetOtherPort(ownerPort);

                if (otherPort?.Module is not IDataModule<IExtensibleItem> otherModule) continue;
                if (!ModuleVisualObjects.TryGetValue(module, out IModuleVisualObject fromModuleVisual)) continue;
                if (!ModuleVisualObjects.TryGetValue(otherModule, out IModuleVisualObject toModuleVisual)) continue;

                IPortVisualObject fromVisual = fromModuleVisual.PortsVisualObjects[module.Ports.ToList().IndexOf(ownerPort)];
                IPortVisualObject toVisual = toModuleVisual.PortsVisualObjects[otherModule.Ports.ToList().IndexOf(otherPort)];
                PortAligner.AlignPorts(fromVisual, toVisual);
            }
        }

        protected virtual IObjectPool<IModuleVisualObject> GetObjectPool(IModuleVisualObjectProvider moduleVisualObjectProvider)
        {
            return UseInGameObjectPool ? moduleVisualObjectProvider.GamePool : moduleVisualObjectProvider.EditorPool;
        }

        protected virtual void HandleModuleAdded(IDataModule<IExtensibleItem> extensibleModule,IModuleVisualObjectProvider moduleVisualObjectProvider)
        {
            
        }
        
        protected virtual void HandleModuleRemoved(IDataModule<IExtensibleItem> extensibleModule)
        {
            
        }

        protected virtual void HandleConnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            
        }
        
        protected virtual void HandleDisconnected(IPortVisualObject portA, IPortVisualObject portB)
        {
            
        }
        
        public virtual void Dispose()
        {
            foreach ((IDataModule<IExtensibleItem> module, IModuleVisualObject visualObject) in ModuleVisualObjects)
            {
                GetObjectPool(module.Value.Extensions.Get<IModuleVisualObjectProvider>()).Release(visualObject);
            }
        }
    }
}