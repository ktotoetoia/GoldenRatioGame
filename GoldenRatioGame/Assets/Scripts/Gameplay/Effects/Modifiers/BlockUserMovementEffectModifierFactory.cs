using UnityEngine;

namespace IM.Effects
{
    [CreateAssetMenu(menuName = "Effects/Modifiers/Block User Movement")]
    public class BlockUserMovementEffectModifierFactory : EffectModifierFactory
    {
        [SerializeField] private bool _blockUserMovement = true;
        
        public override IEffectModifier Create(IEffectContext context) => new BlockUserMovementEffectModifier {BlockUserMovement = _blockUserMovement};
    }
}