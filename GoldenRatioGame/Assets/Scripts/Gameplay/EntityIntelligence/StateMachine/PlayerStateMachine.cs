using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Interactions;
using IM.LifeCycle;
using IM.Modules;
using IM.Movement;
using IM.StateMachines;
using IM.Values;
using UnityEngine;

namespace IM
{
    public class PlayerStateMachine : MonoBehaviour, IPausable
    {
        private IMoveInVector _movement;
        private IInteractor _interactor;
        private IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private IState _movementState;
        private IState _abilityUseState;
        private IState _generalState;
        private InteractionState _interactionState;
        private IState _graphEditingState;

        private ITransition _generalToGraphEditing;
        private ITransition _graphEditingToGeneral;
        private ITransition _generalToInteraction;
        private ITransition _interactionToGeneral;
        
        private IStateMachine _stateMachine;
        private IModuleEntity _entity;

        public Func<Vector2> ProvideMovementDirection { get; set; } = () => default;
        public Func<bool> ShouldTryInteract { get; set; } = () => false;
        public Func<IEnumerable<IAbilityReadOnly>, IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>>>
            ResolveRequestedAbilities { get; set; } = x => new KeyValuePair<IAbilityReadOnly, UseContext>[] { };
        public Func<bool> ShouldTryStartEditing { get; set; } = () => false;
        public Func<bool> ShouldTryStopEditing { get; set; } = () => false;
        public Action<IModuleEditingContext> EditStarted { get; set; } = x => { };
        public Action EditEnded { get; set; } = () => { };
        
        public bool Paused { get; set; }
        
        private void Awake()
        {
            _movement = GetComponent<IMoveInVector>();
            _abilityUser = GetComponent<IAbilityUser<IAbilityPoolReadOnly>>();
            _interactor = GetComponent<IInteractor>();
            _entity = GetComponent<IModuleEntity>();
            
            _movementState = new MovementState(_movement,() => ProvideMovementDirection());
            _abilityUseState = new AbilityUseState(_abilityUser, x => ResolveRequestedAbilities(x));
            _generalState = new CompositeState(new [] {_movementState, _abilityUseState});
            _interactionState = new InteractionState(_interactor, ResolveInteraction);
            _graphEditingState = new GraphEditingState(_entity,x => EditStarted(x), () => EditEnded());
            
            _generalToGraphEditing = new Transition(_generalState,_graphEditingState,()=> ShouldTryStartEditing());
            _graphEditingToGeneral = new Transition(_graphEditingState,_generalState,() => ShouldTryStopEditing());
            _generalToInteraction = new Transition(_generalState, _interactionState, CanInteract);
            _interactionToGeneral = new Transition(_interactionState, _generalState, CanExitInteraction);

            _generalState.AddTransition(_generalToGraphEditing);
            _generalState.AddTransition(_generalToInteraction);
            _graphEditingState.AddTransition(_graphEditingToGeneral);
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
            if (!ShouldTryInteract()) return false;
            
            _lastInteractable = _interactor.GetAvailableInteractions().FirstOrDefault();
                
            return _lastInteractable != null;

        }
        
        private bool CanExitInteraction()
        {
            return _interactionState.Process.Progress != InteractionProgress.Running;
        }
    }
}