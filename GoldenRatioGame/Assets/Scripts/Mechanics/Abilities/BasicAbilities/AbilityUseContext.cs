using UnityEngine;

namespace IM.Abilities
{
    public readonly struct AbilityUseContext
    {
        public readonly Vector2 TargetWorldPosition;
        public readonly Vector2 EntityPosition;

        public AbilityUseContext(Vector2 targetWorldPosition, Vector2 entityPosition)
        {
            TargetWorldPosition = targetWorldPosition;
            EntityPosition = entityPosition;
        }
    }
}