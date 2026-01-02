using IM.Graphs;
using UnityEngine;

namespace IM.Visuals
{
    public class TransformPortAligner
    {
        private readonly ITraversal _traversal = new BreadthFirstTraversal();

        public void Align(ITransformModule anchor)
        {
            foreach ((ITransformModule module, ITransformPort port) in _traversal.EnumerateModules<ITransformModule,ITransformPort>(anchor))
            {
                if(port == null) continue;
                
                AlignModules(port.Connection.GetOtherPort(port) as ITransformPort,port);
            }
        }

        private void AlignModules(ITransformPort input, ITransformPort output)
        {
            IHierarchyTransform outputT = output.Module.HierarchyTransform;
            IHierarchyTransform inputT  = input.Module.HierarchyTransform;

            Vector3 outputLocal = Vector3.Scale(output.Transform.LocalPosition, outputT.LossyScale);
            Vector3 inputLocal  = Vector3.Scale(input.Transform.LocalPosition, inputT.LossyScale);

            Vector3 outputWorldPos = outputT.Position + outputT.Rotation * outputLocal;

            Quaternion outputWorldRot = outputT.Rotation * output.Transform.LocalRotation;
            Quaternion inputWorldRot  = inputT.Rotation * input.Transform.LocalRotation;

            Quaternion deltaRot =
                Quaternion.FromToRotation(inputWorldRot * Vector3.forward,
                    -(outputWorldRot * Vector3.forward));

            Quaternion desiredRotation = deltaRot * inputT.Rotation;

            Vector3 rotatedInputRelPos = desiredRotation * inputLocal;

            Vector3 desiredPosition = outputWorldPos - rotatedInputRelPos;

            inputT.Position = desiredPosition;
            inputT.Rotation = desiredRotation;
        }
    }
}