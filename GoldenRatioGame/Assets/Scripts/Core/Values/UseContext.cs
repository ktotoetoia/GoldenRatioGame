using UnityEngine;

namespace IM.Values
{
    public readonly struct UseContext
    {
        public readonly Vector3 TargetWorldPosition;
        public readonly Vector3 EntityPosition;

        public UseContext(Vector3 targetWorldPosition, Vector3 entityPosition)
        {
            TargetWorldPosition = targetWorldPosition;
            EntityPosition = entityPosition;
        }

        public Vector3 GetDirection()
        {
            return TargetWorldPosition - EntityPosition;
        }
    }
}