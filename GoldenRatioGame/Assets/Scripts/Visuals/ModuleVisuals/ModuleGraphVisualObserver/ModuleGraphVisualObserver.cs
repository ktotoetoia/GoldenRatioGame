using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserver : MonoBehaviour, IModuleGraphSnapshotObserver, IModuleGraphStructureUpdater
    {
        private readonly IHierarchyTransform _globalTransform = new HierarchyTransform();
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        private readonly IPortAligner _portAligner = new PortAligner();
        private readonly Dictionary<IGameModule, IModuleVisualObject> _moduleVisuals = new();
        private ModuleGraphSnapshotDiffer _snapshotDiffer;
        private IVectorMovement _vectorMovement;

        private ModuleGraphSnapshotDiffer SnapshotDiffer =>
            _snapshotDiffer ??= new ModuleGraphSnapshotDiffer
            {
                OnModuleAdded = HandleModuleAdded,
                OnModuleRemoved = HandleModuleRemoved,
                OnConnected = HandleConnected,
            };

        private void Awake()
        {
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_vectorMovement.MovementDirection.x != 0)
            {
                _globalTransform.LocalScale =
                    _vectorMovement.MovementDirection.x > 0
                        ? Vector3.one
                        : new Vector3(-1f, 1f, 1f);
            }

            _globalTransform.Position = transform.position;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            SnapshotDiffer.OnGraphUpdated(graph);
        }

        private void HandleModuleAdded(IModule module)
        {
            if (module is not IGameModule gameModule)
                throw new ArgumentException("Observer supports only IGameModule");
            if (_moduleVisuals.ContainsKey(gameModule))
                throw new ArgumentException($"Module ({gameModule}) already added");
            if (!gameModule.Extensions.TryGetExtension(out IModuleVisual moduleVisual))
                throw new ArgumentException("IModuleVisual extension required");

            IModuleVisualObject visualObject = moduleVisual.CreateModuleVisualObject();
            _moduleVisuals.Add(gameModule, visualObject);
            _globalTransform.AddChildKeepLocal(visualObject.Transform);
            visualObject.ModuleGraphStructureUpdater = this;

            visualObject.Visibility = true;
        }

        private void HandleModuleRemoved(IModule module)
        {
            if (module is not IGameModule gameModule)
                throw new ArgumentException("Observer supports only IGameModule");
            if (!_moduleVisuals.Remove(gameModule, out IModuleVisualObject visualObject))
                throw new ArgumentException($"Module ({gameModule}) was not added");
            
            visualObject.Visibility = false;
            visualObject.ModuleGraphStructureUpdater = null;
            _globalTransform.RemoveChild(visualObject.Transform);
            visualObject.Dispose();
        }

        private void HandleConnected(IConnection connection)
        {
            if (connection.Port1.Module is not IGameModule moduleA ||
                connection.Port2.Module is not IGameModule moduleB)
                throw new ArgumentException("Observer supports only IGameModule");

            _portAligner.AlignPorts(
                _moduleVisuals[moduleA].GetPortVisual(connection.Port1),
                _moduleVisuals[moduleB].GetPortVisual(connection.Port2));
        }

        public void OnPortTransformChanged(IPort port)
        {
            foreach ((IGameModule module, IPort viaPort) in
                     _traversal.EnumerateModulesAlongConnection<IGameModule, IPort>(port))
            {
                IPort otherPort = viaPort.Connection.GetOtherPort(viaPort);
                if (otherPort?.Module is not IGameModule otherModule)
                    continue;

                if (_moduleVisuals[module].GetPortVisual(viaPort) is not { } fromVisual)
                    continue;

                if (_moduleVisuals[otherModule].GetPortVisual(otherPort) is not { } toVisual)
                    continue;
                
                _portAligner.AlignPorts(fromVisual, toVisual);
            }
        }
    }
}