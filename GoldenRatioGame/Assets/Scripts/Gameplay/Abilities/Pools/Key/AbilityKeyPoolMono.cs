using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPoolMono : MonoBehaviour, IKeyAbilityPool, IAbilityPoolEvents
    {
        [SerializeField] private List<KeyCode> _keys = new()
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Q,
            KeyCode.E
        };
        private KeyAbilityPool _keyAbilityPool;
        
        public IReadOnlyDictionary<KeyCode, IAbilityReadOnly> KeyMap => _keyAbilityPool.KeyMap;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _keyAbilityPool.Abilities;
        
        public event Action<IAbilityReadOnly> AbilityAdded
        {
            add => _keyAbilityPool.AbilityAdded += value;
            remove => _keyAbilityPool.AbilityAdded -= value;
        }

        public event Action<IAbilityReadOnly> AbilityRemoved 
        {
            add => _keyAbilityPool.AbilityRemoved += value;
            remove => _keyAbilityPool.AbilityRemoved -= value;
        }

        private void Awake()
        {
            _keyAbilityPool = new KeyAbilityPool(_keys);
        }

        public bool Contains(IAbilityReadOnly ability) => _keyAbilityPool.Contains(ability);
        public void AddAbility(IAbilityReadOnly ability) => _keyAbilityPool.AddAbility(ability);
        public void RemoveAbility(IAbilityReadOnly ability) => _keyAbilityPool.RemoveAbility(ability);
    }
}