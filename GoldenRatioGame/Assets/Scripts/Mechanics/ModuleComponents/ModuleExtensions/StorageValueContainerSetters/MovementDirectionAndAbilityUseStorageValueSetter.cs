using System;
using System.Collections.Generic;
using System.Linq;
using IM.Abilities;
using IM.Events;
using IM.Graphs;
using IM.Modules;
using IM.Movement;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementDirectionAndAbilityUseStorageValueSetter : MonoBehaviour, IModuleGraphSnapshotObserver
    {
        [SerializeField] private GameObject _movementAndAbilityEventsSource;
        [SerializeField] private string _valueStorageTag = DirectionConstants.Focus;
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
            if(CheckAbility()) return;
            
            CheckMovement();
        }

        private bool CheckAbility()
        {
            if (Time.time < _abilityFocusTime + _lastUseTime)
            {
                SetDirectionToContainers(DirectionUtils.GetEnumDirection(_focusPointProvider.GetFocusDirection()));

                return true;
            }
            
            return false;
        }

        private void CheckMovement()
        {
            if (_movement.Direction == Vector2.zero || _containers == null) return;
            
            SetDirectionToContainers(DirectionUtils.GetEnumDirection(_movement.Direction));
        }
        
        public void OnGraphUpdated(IModuleGraphReadOnly graph)
        {
            if(graph == null) throw new ArgumentNullException(nameof(graph));

            _containers = graph.Modules.OfType<IExtensibleModule>().SelectMany(x=> x.Extensions.GetAll<IValueStorageContainer>()).ToList();
            
            SetDirectionToContainers(Direction.Right);
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

        private void SetDirectionToContainers(Direction direction) =>
            _containers.ForEach(x => x.GetOrCreate<Direction>(_valueStorageTag).Value = direction);
    }
}