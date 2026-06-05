using IM.Abilities;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class MovementAndFocusDirectionStorageValueOverrider : MonoBehaviour, IFocusDirectionOverrider
    {
        [SerializeField] private string _valueStorageFocusTag = DirectionConstants.Focus;
        [SerializeField] private string _valueStorageMovementTag = DirectionConstants.Movement;
        [SerializeField] private GameObject _moduleVisualMapSource;
        private IModuleVisualMap _moduleVisualMap;
        private IFocusProvider _abilityFocusProvider;
        private float _abilityStartTime = float.MinValue;
        private float _abilityEndTime = float.MinValue;
        private float _customOverrideStartTime = float.MinValue;
        private float _customOverrideEndTime = float.MinValue;
        private Direction _customOverrideDirection;

        public Direction LastFocusDirection { get; private set; }
        public Direction LastMovementDirection { get; private set; }

        private void Awake()
        {
            _moduleVisualMap = _moduleVisualMapSource.GetComponent<IModuleVisualMap>();
        }

        public void SetMovementDirection(Direction direction)
        {
            LastMovementDirection = direction;
            SetDirectionToContainers(direction, _valueStorageMovementTag);

            SetFocusDirection(ResolveFocusDirection(direction));
        }

        public void SetAbilityFocusPoint(IFocusProvider focusProvider)
        {
            if (focusProvider == null) return;

            _abilityFocusProvider = focusProvider;
            _abilityStartTime = Time.time;
            _abilityEndTime = Time.time + focusProvider.FocusTime;
        }

        public void OverrideFocusDirection(Vector2 direction, float duration)
        {
            _customOverrideDirection = DirectionUtils.GetEnumDirection(direction);
            _customOverrideStartTime = Time.time;
            _customOverrideEndTime = Time.time + duration;
        }
        private Direction ResolveFocusDirection(Direction fallbackDirection)
        {
            bool abilityActive = Time.time < _abilityEndTime && _abilityFocusProvider != null;
            bool customActive = Time.time < _customOverrideEndTime;

            if (!abilityActive && !customActive) return fallbackDirection;

            if (abilityActive && customActive)
            {
                return _customOverrideStartTime > _abilityStartTime ? _customOverrideDirection : DirectionUtils.GetEnumDirection(_abilityFocusProvider.GetFocusDirection());
            }

            if (customActive) return _customOverrideDirection;

            return DirectionUtils.GetEnumDirection(_abilityFocusProvider.GetFocusDirection());
        }

        private void SetFocusDirection(Direction direction)
        {
            LastFocusDirection = direction;
            SetDirectionToContainers(direction, _valueStorageFocusTag);
        }

        private void SetDirectionToContainers(Direction direction, string tag)
        {
            foreach (IModuleVisualObject visual in _moduleVisualMap.ModuleToVisualObjects.Values)
            {
                visual.ValueStorageContainer.GetOrCreate<Direction>(tag).Value = direction;
            }
        }

        private void OnDisable()
        {
            SetFocusDirection(Direction.None);
            SetDirectionToContainers(Direction.None, _valueStorageMovementTag);
        }
    }
}