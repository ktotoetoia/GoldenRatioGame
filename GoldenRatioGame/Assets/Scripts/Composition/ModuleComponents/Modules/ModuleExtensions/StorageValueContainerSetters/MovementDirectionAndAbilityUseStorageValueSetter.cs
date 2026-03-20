using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Events;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Common;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementDirectionAndAbilityUseStorageValueSetter : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private GameObject _movementAndAbilityEventsSource;
        [SerializeField] private string _valueStorageFocusTag = DirectionConstants.Focus;
        [SerializeField] private string _valueStorageMovementTag = DirectionConstants.Movement;
        private List<IValueStorageContainer> _containers;
        private IVectorMovement _movement;
        private IAbilityUserEvents _abilityUserEvents;
        private float _lastUseTime = int.MinValue;
        private float _abilityFocusTime;
        private IFocusPointProvider _focusPointProvider;
        
        private void Awake()
        {
            _movement = _movementAndAbilityEventsSource.GetComponent<IVectorMovement>();
            _abilityUserEvents = _movementAndAbilityEventsSource.GetComponent<IAbilityUserEvents>();
            _abilityUserEvents.OnAbilityStarted += OnAbilityStarted;
        }

        private void Update()
        {
            if(_containers == null) return;
            
            SetDirectionToContainers(DirectionUtils.GetEnumDirection(_movement.Direction),_valueStorageMovementTag);
            if(CheckAbility()) return;
            
            SetDirectionToContainers(DirectionUtils.GetEnumDirection(_movement.Direction),_valueStorageFocusTag);
        }

        private bool CheckAbility()
        {
            if (Time.time < _abilityFocusTime + _lastUseTime)
            {
                SetDirectionToContainers(DirectionUtils.GetEnumDirection(_focusPointProvider.GetFocusDirection()),_valueStorageFocusTag);

                return true;
            }
            
            return false;
        }
        
        public void OnSnapshotChanged(IModuleGraphReadOnly graph)
        {
            if(graph == null) throw new ArgumentNullException(nameof(graph));

            _containers = graph.Modules.OfType<IExtensibleModule>().SelectMany(x=> x.Extensions.GetAll<IValueStorageContainer>()).ToList();
            
            SetDirectionToContainers(Direction.Right,_valueStorageFocusTag);
        }

        private void OnAbilityStarted(IAbilityReadOnly ability)
        {
            if (ability is IFocusPointProvider focusPoint)
            {
                _abilityFocusTime = focusPoint.FocusTime;
                _lastUseTime = Time.time;
                _focusPointProvider = focusPoint;
            }
        }

        private void SetDirectionToContainers(Direction direction,string tag) =>
            _containers.ForEach(x => x.GetOrCreate<Direction>(tag).Value = direction);
    }
}