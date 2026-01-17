using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class ModuleGraphVisualObserver : MonoBehaviour, IModuleGraphSnapshotObserver,IModuleGraphStructureUpdater
    {
        private readonly List<IGameModule> _transformModules = new();
        private readonly IHierarchyTransform _globalTransform = new HierarchyTransform();
        private readonly ITraversal _traversal = new BreadthFirstTraversal();
        private ModuleGraphSnapshotDiffer _graphSnapshotDiffer;
        private IVectorMovement _vectorMovement;
        
        private ModuleGraphSnapshotDiffer GraphSnapshotDiffer => _graphSnapshotDiffer??= new ModuleGraphSnapshotDiffer()
        {
            OnModuleAdded = OnModuleAdded,
            OnModuleRemoved = OnModuleRemoved,
            OnConnected = OnConnected,
        };

        private void Awake()
        {
            _vectorMovement = GetComponent<IVectorMovement>();
        }

        private void Update()
        {
            if (_vectorMovement.MovementDirection.x != 0)
                _globalTransform.LocalScale = _vectorMovement.MovementDirection.x > 0 ? Vector3.one : new Vector3(-1, 1, 1);
            
            _globalTransform.Position = transform.position;
        }

        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            GraphSnapshotDiffer.OnGraphUpdated(graph);
        }
        
        private void OnModuleAdded(IModule addedModule)
        {
            if(addedModule is not IGameModule module) 
                throw new ArgumentException("this observer only supports IGameModule");
            if (_transformModules.Contains(module))
                throw new ArgumentException($"module ({module}) was already added to observer");
            
            _transformModules.Add(module);
            
            if(module.Extensions.TryGetExtension(out IPortTransformChanger changer)) 
                changer.ModuleGraphStructureUpdater = this;

            module.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.Visibility = true;
            _globalTransform.AddChildKeepLocal(module.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.Transform);
        }

        private void OnModuleRemoved(IModule removedModule)
        {
            if(removedModule is not IGameModule module) 
                throw new ArgumentException("this observer only supports IGameModule");
            if(!_transformModules.Remove(module))
                throw new ArgumentException($"module ({module}) was not added to observer");
            
            if(module.Extensions.TryGetExtension(out IPortTransformChanger changer)) 
                changer.ModuleGraphStructureUpdater = null;

            module.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.Visibility = false;
            _globalTransform.RemoveChild(module.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.Transform);
        }

        private void OnConnected(IConnection connection)
        {
            if(connection.Port1.Module is not IGameModule module1 ||  connection.Port2.Module is not IGameModule module2)
                throw new ArgumentException("this observer only supports IGameModule");

            AlignPorts(
                module1.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.GetPortVisual(connection.Port1),
                module2.Extensions.GetExtension<IModuleVisual>().ReferenceModuleVisualObject.GetPortVisual(connection.Port2));
        }

        public void OnPortTransformChanged(IPort port)
        {
            foreach ((IGameModule module, IPort via) in _traversal.EnumerateModulesAlongConnection<IGameModule, IPort>(port))
            {
                IModuleVisual fromVisualExt = module.Extensions.GetExtension<IModuleVisual>();
                if (fromVisualExt == null) continue;

                IPort otherPort = via.Connection.GetOtherPort(via);
                if (otherPort?.Module is not IGameModule otherModule) continue;
                
                IModuleVisual toVisualExt = otherModule.Extensions.GetExtension<IModuleVisual>();
                if (toVisualExt == null) continue;

                if (fromVisualExt.ReferenceModuleVisualObject.GetPortVisual(via) is not { } fromPortVisual) continue;
                if (toVisualExt.ReferenceModuleVisualObject.GetPortVisual(otherPort) is not { } toPortVisual) continue;

                AlignPorts(toPortVisual, fromPortVisual);
            }
        }
        
        private void AlignPorts(IPortVisualObject portToMove, IPortVisualObject anchorPort)
        {
            Vector3 inputLocal = Vector3.Scale(portToMove.Transform.LocalPosition, portToMove.OwnerVisualObject.Transform.LossyScale);
            Vector3 outputLocal = Vector3.Scale(anchorPort.Transform.LocalPosition, anchorPort.OwnerVisualObject.Transform.LossyScale);

            Vector3 outputWorldPos =
                anchorPort.OwnerVisualObject.Transform.Position + anchorPort.OwnerVisualObject.Transform.Rotation * outputLocal;

            float inputWorldZ =
                (portToMove.OwnerVisualObject.Transform.Rotation * portToMove.Transform.LocalRotation).eulerAngles.z;

            float outputWorldZ =
                (anchorPort.OwnerVisualObject.Transform.Rotation * anchorPort.Transform.LocalRotation).eulerAngles.z;

            float desiredInputWorldZ = outputWorldZ + 180f;

            float deltaZ = Mathf.DeltaAngle(inputWorldZ, desiredInputWorldZ);

            float newInputWorldZ = portToMove.OwnerVisualObject.Transform.Rotation.eulerAngles.z + deltaZ;
            Quaternion desiredRotation = Quaternion.Euler(0f, 0f, newInputWorldZ);

            Vector3 rotatedInputLocal = desiredRotation * inputLocal;
            Vector3 desiredPosition = outputWorldPos - rotatedInputLocal;

            portToMove.OwnerVisualObject.Transform.Position = desiredPosition;
            portToMove.OwnerVisualObject.Transform.Rotation = desiredRotation;
        }
    }
}