using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Entities
{
    public class RangeBasedModuleEntityInteractor : MonoBehaviour, IInteractor, IRequireInteractionProvider
    {
        [Header("Settings")]
        [SerializeField] private GameObject _subInteractorSource;
        [SerializeField] private float _maxInteractionRange = 0.5f; 
        [SerializeField] private float _interactionTime = 0.3f;
        private List<ISubInteractor> _subInteractors;
        private IEntity _entity;
        private InteractionProcess _process;
        private Coroutine _interactionRoutine;
        
        public bool IsInteracting => _interactionRoutine != null;
        public IInteractable CurrentTarget { get; private set; }
        public IInteractionProvider InteractionProvider { get; set; }

        private void Awake()
        {   
            if (_subInteractorSource == null) 
                throw new MissingComponentException($"{nameof(_subInteractorSource)} is required on {gameObject.name}");

            _subInteractors = _subInteractorSource.GetComponents<ISubInteractor>().ToList();
            _entity = GetComponent<IEntity>();
        }

        public IEnumerable<IInteractable> GetAvailableInteractions()
        {
            return InteractionProvider?.GetAvailableInteractions(_entity)
                       .Where(CanInteract).OrderBy(x => Vector3.Distance(x.GameObject.transform.position, _entity.GameObject.transform.position)) ?? Enumerable.Empty<IInteractable>();
        }

        public IInteractionProcess InteractWithFirst()
        {
            var target = GetAvailableInteractions().FirstOrDefault();
            
            return target != null ? InteractWith(target) : InteractionProcess.Failed;
        }

        public bool CanInteract(IInteractable target) => CanInteract(target, out _);

        private bool CanInteract(IInteractable target, out ISubInteractor interactor)
        {
            interactor = null;
            if (target == null || IsInteracting || 
                Vector3.Distance(target.GameObject.transform.position, _entity.GameObject.transform.position) >_maxInteractionRange) return false;
            
            interactor = _subInteractors.FirstOrDefault(x => x.CanInteract(target));
            return interactor != null;
        }
        
        public IInteractionProcess InteractWith(IInteractable target)
        {
            if (!CanInteract(target, out var interactor)) 
                return InteractionProcess.Failed;

            return StartInteraction(target, interactor);
        }

        private IInteractionProcess StartInteraction(IInteractable target, ISubInteractor interactor)
        {
            CurrentTarget = target;
            var process = new InteractionProcess(target, _interactionTime, Time.time);
            
            _interactionRoutine = StartCoroutine(InteractionTimer(process, interactor));
            
            return process;
        }
        
        private IEnumerator InteractionTimer(InteractionProcess process, ISubInteractor interactor)
        {
            _process = process;
            yield return new WaitForSeconds(_interactionTime);
            
            interactor.Interact(_process.Target);
            _process.Target.OnInteract(_entity);
            _process.CallOnComplete();

            ClearState();
        }

        public void Interrupt()
        {
            if (!IsInteracting) return;

            StopCoroutine(_interactionRoutine);
            _process.CallOnInterrupted();
            
            ClearState();
        }

        private void ClearState()
        {
            _interactionRoutine = null;
            CurrentTarget = null;
        }
    }
}