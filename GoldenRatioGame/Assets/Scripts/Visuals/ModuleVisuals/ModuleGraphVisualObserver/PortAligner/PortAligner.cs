using UnityEngine;

namespace IM.Visuals
{
    public class PortAligner : IPortAligner
    {
        public void AlignPorts(IPortVisualObject portToMove, IPortVisualObject anchorPort)
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