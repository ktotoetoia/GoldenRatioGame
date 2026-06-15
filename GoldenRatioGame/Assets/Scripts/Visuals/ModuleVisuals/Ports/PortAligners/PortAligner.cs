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

        Quaternion anchorCombinedRot = anchorPort.Transform.Rotation * Quaternion.Euler(0, 0, anchorPort.Rotation);

        Quaternion portCombinedRot = portToMove.Transform.Rotation * Quaternion.Euler(0, 0, portToMove.Rotation);

        Quaternion portLocalCombinedRot = Quaternion.Inverse(owner.Rotation) * portCombinedRot;

        Quaternion result = anchorCombinedRot * Quaternion.Inverse(portLocalCombinedRot);

        result *= Quaternion.Euler(0, 0, 180);
        Debug.DrawLine(anchorPort.Transform.Position, anchorPort.Transform.Position +  (result * anchorPort.Transform.Position).normalized, Color.yellow,0.3f);

        return result;
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