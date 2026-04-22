using UnityEngine;

namespace IM.Abilities
{
    public class DefaultAbilityAnchorPositionProvider : MonoBehaviour, IAbilityAnchorPositionProvider
    {
        public Vector3 GetAnchorPosition(IAbilityReadOnly ability) => transform.position;
    }
}