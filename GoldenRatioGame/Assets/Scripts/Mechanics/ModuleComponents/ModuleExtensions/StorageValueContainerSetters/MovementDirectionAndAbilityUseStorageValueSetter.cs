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
        private AbilityUseContext _lastUseContext;
        private float _lastUseTime = int.MinValue;
        private float _abilityFocusTime;
        
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
                SetDirectionToContainers(DirectionUtils.GetEnumDirection(_lastUseContext.GetDirection()));

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
            if (ability.AbilityDescriptorsRegistry.TryGet(
                    out IAbilityFocusTimeDescriptor desc))
            {
                _abilityFocusTime = desc.FocusTime;
                _lastUseTime = Time.time;
                _lastUseContext = (ability as IUseContextAbility)?.LastUsedContext ?? default;
            }
        }

        private void SetDirectionToContainers(Direction direction) =>
            _containers.ForEach(x => x.GetOrCreate<Direction>(_valueStorageTag).Value = direction);
    }
}