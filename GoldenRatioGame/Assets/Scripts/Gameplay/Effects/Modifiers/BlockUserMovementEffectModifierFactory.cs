using System;
using UnityEngine;

namespace IM.Effects
{
    [Serializable]
    public class BlockUserMovementEffectModifierFactory : IEffectModifierFactory
    {
        [SerializeField] private bool _blockUserMovement = true;
        
        public IEffectModifier Create(IEffectContext context) => new BlockUserMovementEffectModifier {BlockUserMovement = _blockUserMovement};
    }
}