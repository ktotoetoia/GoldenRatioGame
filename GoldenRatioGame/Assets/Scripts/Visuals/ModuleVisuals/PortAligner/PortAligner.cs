using IM.Transforms;
using UnityEngine;

namespace IM.Visuals
{
    public class PortAligner : IPortAligner
    {
        public void AlignPorts(IPortVisualObject portToMove, IPortVisualObject anchorPort)
        {
            ITransform owner = portToMove.OwnerVisualObject.Transform;

            owner.Rotation = GetRotation(portToMove, anchorPort);
            owner.Position = GetPosition(portToMove, anchorPort, owner.Rotation);
        }
        
        private Quaternion GetRotation(IPortVisualObject portToMove, IPortVisualObject anchorPort)
        {
            ITransform owner = portToMove.OwnerVisualObject.Transform;

            Quaternion portLocal =
                Quaternion.Inverse(owner.Rotation) *
                portToMove.Transform.Rotation;

            return anchorPort.Transform.Rotation *
                   Quaternion.Inverse(portLocal) * Quaternion.Euler(0,0,180);
        }
        
        private Vector3 GetPosition(IPortVisualObject portToMove, IPortVisualObject anchorPort, Quaternion ownerRot)
        {
            Vector3 portLocalPos = portToMove.Transform.LocalPosition;
            Vector3 ownerScale = portToMove.OwnerVisualObject.Transform.LossyScale;
            Vector3 offset = ownerRot * Vector3.Scale(ownerScale, portLocalPos);

            return anchorPort.Transform.Position - offset;
        }
    }
}