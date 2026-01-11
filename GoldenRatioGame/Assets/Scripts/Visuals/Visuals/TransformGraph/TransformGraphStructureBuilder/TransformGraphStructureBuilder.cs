using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class TransformGraphStructureBuilder : IModuleGraphObserver,IPortTransformChanger
    {
        private readonly List<ITransformModule> _transformModules = new();
        private readonly IHierarchyTransform _globalTransform;
        private readonly ITraversal _traversal = new BreadthFirstTraversal();

        public ITransform GlobalTransform => _globalTransform;

        public TransformGraphStructureBuilder() : this(new HierarchyTransform())
        {
            
        }
        
        public TransformGraphStructureBuilder(IHierarchyTransform globalTransform)
        {
            _globalTransform = globalTransform;
        }

        public void OnModuleAdded(IModule addedModule)
        {
            if(addedModule is not IPortTransformModule module) 
                throw new ArgumentException("this observer only supports ITransformModule");
            if (_transformModules.Contains(module))
                throw new ArgumentException($"module ({module}) was already added to structure builder");
            module.TransformChanger = this;
            _transformModules.Add(module);
            _globalTransform.AddChild(module.HierarchyTransform);
        }

        public void OnModuleRemoved(IModule removedModule)
        {
            if(removedModule is not IPortTransformModule module) 
                throw new ArgumentException("this observer only supports ITransformModule");
            module.TransformChanger = null;
            
            _transformModules.Remove(module);
            _globalTransform.RemoveChild(module.HierarchyTransform);
        }

        public void OnConnected(IConnection connection)
        {            
            if(connection.Port1 is not ITransformPort input || connection.Port2 is not ITransformPort output)
                throw new ArgumentException("this observer only supports ITransformPort");
            
            AlignPorts(input, output);
        }
        
        private void AlignPorts(ITransformPort input, ITransformPort output)
        {
            IHierarchyTransform inputT = input.Module.HierarchyTransform;
            IHierarchyTransform outputT = output.Module.HierarchyTransform;

            Vector3 inputLocal = Vector3.Scale(input.Transform.LocalPosition, inputT.LossyScale);
            Vector3 outputLocal = Vector3.Scale(output.Transform.LocalPosition, outputT.LossyScale);

            Vector3 outputWorldPos =
                outputT.Position + outputT.Rotation * outputLocal;

            float inputWorldZ =
                (inputT.Rotation * input.Transform.LocalRotation).eulerAngles.z;

            float outputWorldZ =
                (outputT.Rotation * output.Transform.LocalRotation).eulerAngles.z;

            float desiredInputWorldZ = outputWorldZ + 180f;

            float deltaZ = Mathf.DeltaAngle(inputWorldZ, desiredInputWorldZ);

            float newInputWorldZ = inputT.Rotation.eulerAngles.z + deltaZ;
            Quaternion desiredRotation = Quaternion.Euler(0f, 0f, newInputWorldZ);

            Vector3 rotatedInputLocal = desiredRotation * inputLocal;
            Vector3 desiredPosition = outputWorldPos - rotatedInputLocal;

            inputT.Position = desiredPosition;
            inputT.Rotation = desiredRotation;
        }

        public void OnDisconnected(IConnection connection)
        {
            
        }
        
        public void SetPortPosition(ITransformPort port, Vector3 newPosition)
        {
            port.Transform.Position = newPosition;
            
            AlignSubtree(port);
        }
        
        public void SetPortRotation(ITransformPort port, float zRotation)
        {
            port.Transform.LocalRotation = Quaternion.Euler(0,0,zRotation);
            
            AlignSubtree(port);
        }

        private void AlignSubtree(ITransformPort port)
        {
            foreach ((ITransformModule module, ITransformPort via)  in _traversal.EnumerateModulesAlongConnection<ITransformModule, ITransformPort>(port))
            {
                AlignPorts(via,via.Connection.GetOtherPort(via) as ITransformPort);
            }
        }
    }
}