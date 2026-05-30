using System;
using System.Linq;
using IM.Abilities;
using IM.Events;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementAndFocusDirectionStorageValueSetter : MonoBehaviour, IFocusDirectionSetter
    {
        [SerializeField] private string _valueStorageFocusTag = DirectionConstants.Focus;
        [SerializeField] private string _valueStorageMovementTag = DirectionConstants.Movement;
        private IFocusPointProvider _abilityFocusPointProvider;
        private IModuleEditingContextEditor _editor;
        private float _abilityStartTime = float.MinValue;
        private float _abilityEndTime = float.MinValue;
        private float _customOverrideStartTime = float.MinValue;
        private float _customOverrideEndTime = float.MinValue;
        private Direction _customOverrideDirection;

        public Direction LastFocusDirection { get; private set; }
        public Direction LastMovementDirection { get; private set; }

        private void Awake()
        {
            _editor = GetComponent<IModuleEditingContextEditor>();
        }

        public void SetMovementDirection(Direction direction)
        {
            LastMovementDirection = direction;
            SetDirectionToContainers(direction, _valueStorageMovementTag);

            SetFocusDirection(ResolveFocusDirection(direction));
        }

        public void SetAbilityFocusPoint(IFocusPointProvider focusPointProvider)
        {
            if (focusPointProvider == null) return;

            _abilityFocusPointProvider = focusPointProvider;
            _abilityStartTime = Time.time;
            _abilityEndTime = Time.time + focusPointProvider.FocusTime;
        }

        public void OverrideFocusDirection(Vector2 direction, float duration)
        {
            _customOverrideDirection = DirectionUtils.GetEnumDirection(direction);
            _customOverrideStartTime = Time.time;
            _customOverrideEndTime = Time.time + duration;
        }
        private Direction ResolveFocusDirection(Direction fallbackDirection)
        {
            bool abilityActive = Time.time < _abilityEndTime && _abilityFocusPointProvider != null;
            bool customActive = Time.time < _customOverrideEndTime;

            if (!abilityActive && !customActive) return fallbackDirection;

            if (abilityActive && customActive)
            {
                return _customOverrideStartTime > _abilityStartTime ? _customOverrideDirection : DirectionUtils.GetEnumDirection(_abilityFocusPointProvider.GetFocusDirection());
            }

            if (customActive) return _customOverrideDirection;

            return DirectionUtils.GetEnumDirection(_abilityFocusPointProvider.GetFocusDirection());
        }

        private void SetFocusDirection(Direction direction)
        {
            LastFocusDirection = direction;
            SetDirectionToContainers(direction, _valueStorageFocusTag);
        }

        private void SetDirectionToContainers(Direction direction, string tag)
        {
            foreach (IValueStorageContainer container in _editor.Snapshot.Graph.DataModules.SelectMany(x=> x.Value.Extensions.GetAll<IValueStorageContainer>()?? Array.Empty<IValueStorageContainer>()))
            {
                container.GetOrCreate<Direction>(tag).Value = direction;
            }
        }

        private void OnDisable()
        {
            SetFocusDirection(Direction.None);
            SetDirectionToContainers(Direction.None, _valueStorageMovementTag);
        }
    }
}