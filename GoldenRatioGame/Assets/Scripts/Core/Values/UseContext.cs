using UnityEngine;

namespace IM.Values
{
    public readonly struct UseContext
    {
        public readonly Vector3 TargetWorldPosition;
        public readonly Vector3 AnchorPosition;
        public readonly Vector3 EntityPosition;

        public UseContext(Vector3 targetWorldPosition, Vector3 entityPosition) : this(targetWorldPosition,entityPosition,entityPosition)
        {
            
        }
        
        public UseContext(Vector3 targetWorldPosition, Vector3 entityPosition, Vector3 anchorPosition)
        {
            TargetWorldPosition = targetWorldPosition;
            EntityPosition = entityPosition;
            AnchorPosition = anchorPosition;
        }

        public Vector3 GetDirection()
        {
            return TargetWorldPosition - EntityPosition;
        }
    }
}