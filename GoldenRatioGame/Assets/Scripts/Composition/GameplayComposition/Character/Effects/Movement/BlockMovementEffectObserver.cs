using System.Collections.Generic;
using System.Linq;
using IM.Movement;
using UnityEngine;

namespace IM.Effects
{
    public class BlockMovementEffectObserver : MonoBehaviour, IEffectObserver
    {
        [SerializeField] private GameObject _movementSource;
        private IControllableMovement _movement;
        private readonly List<IBlockUserMovementEffectModifier> _movementBlockingEffects = new();
        
        private void Awake()
        {
            _movement = _movementSource.GetComponent<IControllableMovement>();
        }
        
        public void OnEffectGroupAdded(IEffectGroup group)
        {
            if (group.Modifiers.TryGetAll(out IEnumerable<IBlockUserMovementEffectModifier> modifiers))
            {
                _movementBlockingEffects.AddRange(modifiers);
            }
            
            Evaluate();
        }

        public void OnEffectGroupRemoved(IEffectGroup group)
        {
            if (group.Modifiers.TryGetAll(out IEnumerable<IBlockUserMovementEffectModifier> modifiers))
            {
                foreach(var modifier in modifiers) _movementBlockingEffects.Remove(modifier);
            }
            
            Evaluate();
        }

        private void Evaluate()
        {
            bool blocked = _movementBlockingEffects.Any(x => x.BlockUserMovement);
            
            if (_movement.Active && blocked) _movement.Halt();
            if(!_movement.Active && !blocked) _movement.Activate();
        }
    }
}