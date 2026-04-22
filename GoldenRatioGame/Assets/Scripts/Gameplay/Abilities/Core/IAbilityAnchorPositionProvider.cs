using UnityEngine;

namespace IM.Abilities
{
    public interface IAbilityAnchorPositionProvider
    {
        Vector3 GetAnchorPosition(IAbilityReadOnly ability);
    }
}