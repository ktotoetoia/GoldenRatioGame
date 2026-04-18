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
        private bool _used;
        private IAbilityEvents _contextAbility;
        private IValueStorageContainer _container;
        private IValueStorage<bool> _storage;
        
        private void Awake()
        {
            if (GetComponent<IAbilityExtension>().Ability is not IAbilityEvents useContextAbility)
                throw new Exception($"To use {nameof(SetBoolOnAbilityUsed)}, ability must implement IAbilityEvents");
            
            useContextAbility.AbilityStarted += AbilityStarted;
            _contextAbility = useContextAbility;
            _container = GetComponent<IValueStorageContainer>();
            _storage = _container.GetOrCreate<bool>(_boolName);
        }

        private void Update()
        {
            if (_storage != null) _storage.Value = _used;
            
            _used = false;
        }

        private void AbilityStarted(UseContext context)
        {
            _used = true;
        }

        private void OnDestroy()
        {
            if (_contextAbility != null) _contextAbility.AbilityStarted -= AbilityStarted;
        }
    }
}