using System.Collections.Generic;
using IM.Abilities;
using IM.Base;
using IM.Entities;
using IM.Movement;
using IM.StateMachines;
using UnityEngine;

namespace Tests
{
    public class LegacyInputStateMachineFactory : IFactory<IStateMachine>
    {
        private readonly IEntity _moduleEntity;
        private readonly IMoveInVector _movement;
        private readonly KeyAbilityPoolUserMono  _abilityUser;
        private readonly Camera _gameCamera;

        public LegacyInputStateMachineFactory(IEntity moduleEntity, Camera gameCamera)
        {
            _moduleEntity = moduleEntity;
            _gameCamera = gameCamera;
            _movement = _moduleEntity.GameObject.GetComponent<IMoveInVector>();
            _abilityUser = _moduleEntity.GameObject.GetComponent<KeyAbilityPoolUserMono>();
        }
        
        public IStateMachine Create()
        {
            IState movementState = new MovementState(_movement,
                () => new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            
            _abilityUser.GetAbilityUseContext =x => GetAbilityUseContext();
            
            AbilityUseState abilityUseState = new AbilityUseState(_abilityUser);

            ITransition movementToAbility = new ToAbilityUseTransition(movementState, abilityUseState, () =>
            {
                foreach (KeyValuePair<KeyCode, IAbilityReadOnly> pair in _abilityUser.AbilityPool.KeyMap)
                {
                    if(Input.GetKeyDown(pair.Key)) return pair.Value;
                }

                return null;
            });
            
            Transition abilityToMovement = new Transition(abilityUseState, movementState, () => abilityUseState.Finished);
            
            movementState.AddTransition(movementToAbility);
            abilityUseState.AddTransition(abilityToMovement);
            
            return new StateMachine(movementState);
        }

        private AbilityUseContext GetAbilityUseContext()
        {
            Vector3 mousePosition = _gameCamera.ScreenToWorldPoint(Input.mousePosition) * Vector2.one;
            return new AbilityUseContext(mousePosition, _moduleEntity.GameObject.transform.position);
        }
    }
}