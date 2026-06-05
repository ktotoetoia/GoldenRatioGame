using IM.Abilities;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementAndAbilityDirectionSource : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private GameObject _movementAndAbilityEventsSource;
        [SerializeField] private MovementAndFocusDirectionStorageValueOverrider _target;
        private IVectorMovement _movement;
        private IAbilityUserEvents _abilityUserEvents;

        private void Awake()
        {
            _movement = _movementAndAbilityEventsSource.GetComponent<IVectorMovement>();
            _abilityUserEvents = _movementAndAbilityEventsSource.GetComponent<IAbilityUserEvents>();
        }

        private void Update()
        {
            if (!_target) return;

            var movementDirection = DirectionUtils.GetEnumDirection(_movement.Direction);
            _target.SetMovementDirection(movementDirection);
        }

        private void OnAbilityStarted(IAbilityReadOnly ability)
        {
            if (ability is not IFocusProvider focusPointProvider) return;
            
            _target.SetAbilityFocusPoint(focusPointProvider);
        }

        private void OnEnable()
        {
            _abilityUserEvents.AbilityStarted += OnAbilityStarted;
        }

        private void OnDisable()
        {
            if (_abilityUserEvents == null) return;
            
            _abilityUserEvents.AbilityStarted -= OnAbilityStarted;
        }

        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            _target.OverrideFocusDirection(Vector2.right, 0.1f);
        }
    }
}