using System;
using System.Linq;
using IM.Abilities;
using IM.Entities;
using IM.Movement;
using IM.StateMachines;
using UnityEngine;

namespace IM
{
    public class PlayerStateMachine : MonoBehaviour, IPausable
    {
        private IMoveInVector _movement;
        private IInteractor _interactor;
        private AbilityPoolMonoUser _abilityUser;
        
        private IState _movementState;
        private IState _abilityUseState;
        private IState _generalState;
        private InteractionState _interactionState;
        private ITransition _generalToInteraction;
        private ITransition _interactionToGeneral;
        private IStateMachine _stateMachine;

        public Func<Vector2> ProvideMovementDirection { get; set; } = () => default;
        public Func<AbilityUseContext>  ProvideAbilityUseContext { get; set; }= () => default;
        public Func<bool> ShouldTryInteract { get; set; }= () => false;
        public Func<IAbilityReadOnly,int, KeyCode> ProvideKeyForAbility { get; set; } = (x,y) => default;
        
        public bool Paused { get; set; }
        
        private void Awake()
        {
            _movement = GetComponent<IMoveInVector>();
            _abilityUser = GetComponent<AbilityPoolMonoUser>();
            _interactor = GetComponent<IInteractor>();
            
            _movementState = new MovementState(_movement, () => ProvideMovementDirection());
            _abilityUseState = new AbilityUseState(_abilityUser,() => ProvideAbilityUseContext(),
                x => ProvideKeyForAbility(x,_abilityUser.AbilityPool.Abilities.ToList().IndexOf(x)));
            _generalState = new CompositeState(new [] {_movementState, _abilityUseState});
            _interactionState = new InteractionState(_interactor, ResolveInteraction);
            _generalToInteraction = new Transition(_generalState, _interactionState, CanInteract);
            _interactionToGeneral = new Transition(_interactionState, _generalState, CanExitInteraction);
            
            _generalState.AddTransition(_generalToInteraction);
            _interactionState.AddTransition(_interactionToGeneral);
            
            _stateMachine = new StateMachine(_generalState);
        }

        private void Update()
        {
            if (Paused) return;
            
            _stateMachine.Update();
            _stateMachine.UpdateTransition();
        }

        private void FixedUpdate()
        {
            if(Paused) return;
            
            _stateMachine.FixedUpdate();
        }

        private IInteractable _lastInteractable;
        
        private IInteractable ResolveInteraction() => _lastInteractable;

        private bool CanInteract()
        {
            if (ShouldTryInteract())
            {
                _lastInteractable = _interactor.GetAvailableInteractions().FirstOrDefault();
                
                return _lastInteractable != null;
            }

            return false;
        }
        
        private bool CanExitInteraction()
        {
            return _interactionState.Process.Progress != InteractionProgress.Running;
        }
    }
}