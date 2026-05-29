using System;
using IM.Interactions;
using IM.StateMachines;
using UnityEngine;

namespace IM
{
    public class InteractionState : State
    {
        private readonly IInteractor _interactor;
        private readonly Func<IInteractable> _getTarget;

        public IInteractionProcess Process { get; private set; }
        
        public InteractionState(IInteractor interactor, Func<IInteractable> getTarget)
        {
            _interactor = interactor ??  throw new ArgumentNullException(nameof(interactor));
            _getTarget = getTarget;
        }

        public override void OnEnter()
        {
            IInteractable target = _getTarget();
            
            if (target == null)
            {
                Debug.LogWarning("InteractionState was transitioned to, but there is no target that it can interact with");
                return;
            }

            Process = _interactor.InteractWith(target);
        }

        public override void OnExit()
        {
            Process = null;
        }
    }
}