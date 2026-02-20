using UnityEngine;

namespace IM.Abilities
{
    public readonly struct AbilityUseContext
    {
        public readonly Vector3 TargetWorldPosition;
        public readonly Vector3 EntityPosition;

        public AbilityUseContext(Vector3 targetWorldPosition, Vector3 entityPosition)
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