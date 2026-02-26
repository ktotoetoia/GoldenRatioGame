using IM.Abilities;
using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    public class CurveMovementAbilityMono : MonoBehaviour
    {
        private IControllableMovement _movement;
        private IAbilityUserEvents _events;
        private IBlockUserMovementAbility _blockMovement;
        
        private void Awake()
        {
            _movement = GetComponent<IControllableMovement>();
            _events = GetComponent<IAbilityUserEvents>();
            _events.OnAbilityUsed += (x,y) =>
            {
                x.AbilityDescriptorsRegistry.TryGet(out _blockMovement);

                if (_blockMovement is { BlockUserMovement: true })
                {
                    _movement.Halt();
                    
                    return;
                }
                
                _movement.Activate();
            };
        }
    }
}