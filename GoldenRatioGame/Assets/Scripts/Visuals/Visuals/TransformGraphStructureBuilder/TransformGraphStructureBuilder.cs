using System;
using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class TransformGraphStructureBuilder : IModuleGraphObserver
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
            if(addedModule is not ITransformModule module) 
                throw new ArgumentException($"this observer only supports ITransformModule");
            if (_transformModules.Contains(module))
                throw new ArgumentException($"module ({module}) was already added to structure builder");
            
            _transformModules.Add(module);
            _globalTransform.AddChild(module.HierarchyTransform as IHierarchyTransform);
        }

        public void OnModuleRemoved(IModule removedModule)
        {
            if(removedModule is not ITransformModule module) 
                throw new ArgumentException($"this observer only supports ITransformModule");
            
            _transformModules.Remove(module);
            _globalTransform.RemoveChild(module.HierarchyTransform as IHierarchyTransform);
        }

        public void OnConnected(IConnection connection)
        {            
            if(connection.Input is not ITransformPort input || connection.Output is not ITransformPort output)
                throw new ArgumentException($"this observer only supports ITransformPort");
            IHierarchyTransform outputT = output.Module.HierarchyTransform as IHierarchyTransform;
            IHierarchyTransform inputT = input.Module.HierarchyTransform as IHierarchyTransform;

            Vector3 outputLocal = Vector3.Scale(output.Transform.LocalPosition, outputT.LossyScale);
            Vector3 inputLocal = Vector3.Scale(input.Transform.LocalPosition, inputT.LossyScale);

            Vector3 outputWorldPos = outputT.Position + outputT.Rotation * outputLocal;

            Quaternion outputWorldRot = outputT.Rotation * output.Transform.LocalRotation;
            Quaternion inputWorldRot = inputT.Rotation * input.Transform.LocalRotation;

            Quaternion deltaRot =
                Quaternion.FromToRotation(inputWorldRot * Vector3.forward,
                    -(outputWorldRot * Vector3.forward));

            Quaternion desiredRotation = deltaRot * inputT.Rotation;

            Vector3 rotatedInputRelPos = desiredRotation * inputLocal;

            Vector3 desiredPosition = outputWorldPos - rotatedInputRelPos;

            inputT.Position = desiredPosition;
            inputT.Rotation = desiredRotation;
        }

        public void OnDisconnected(IConnection connection)
        {
            return;
        }
    }
}