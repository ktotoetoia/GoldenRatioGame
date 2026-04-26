using System;
using IM.Abilities;
using IM.Events;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Visuals
{
    public class SetBoolOnAbilityUsed : MonoBehaviour
    {
        [SerializeField] private string _boolName;
        private IAbilityExtension _abilityExtension;
        private IAbilityEvents _lastEvents;
        private IValueStorage<bool> _valueStorage;
        private bool _used;
        
        private void Awake()
        {
            _abilityExtension = GetComponent<IAbilityExtension>();
            _valueStorage =  GetComponent<IValueStorageContainer>().GetOrCreate<bool>(_boolName);
        }

        private void Update()
        {
            UpdateAbility();
            
            if (_valueStorage != null) _valueStorage.Value = _used;
            
            _used = false;
        }
        
        private void UpdateAbility()
        {
            if (_abilityExtension.Ability != _lastEvents && _lastEvents != null)
            {
                _lastEvents.AbilityStarted -= AbilityStarted;
                _lastEvents = null;
            }
            if (_abilityExtension.Ability is IAbilityEvents events && _lastEvents == null)
            {
                _lastEvents = events;
                _lastEvents.AbilityStarted += AbilityStarted;
            }
        }

        private void AbilityStarted(UseContext context)
        {
            _used = true;
        }

        private void OnDestroy()
        {
            if (_lastEvents != null) _lastEvents.AbilityStarted -= AbilityStarted;
        }
    }
}