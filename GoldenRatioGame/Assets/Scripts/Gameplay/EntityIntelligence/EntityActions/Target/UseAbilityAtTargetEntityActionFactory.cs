using System;
using IM.Abilities;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class UseAbilityAtTargetEntityActionFactory : IEntityActionFactory
    {
        [SerializeField] private float _minCooldown = 1f;
        [SerializeField] private float _maxCooldown = 3f;

        public IEntityAction Create(GameObject param1) =>
            new UseAbilityAtTargetEntityAction(
                param1.transform,
                param1.GetComponent<IAbilityUser<IAbilityPoolReadOnly>>(),
                param1.GetComponent<IMemoryContainer>(),
                _minCooldown,
                _maxCooldown
            );
    }
}