using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPool : IKeyAbilityPool, IAbilityPoolEvents
    {
        private readonly Dictionary<KeyCode, IAbilityReadOnly> _map = new();
        private readonly IEnumerable<KeyCode> _order;

        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _map.Values;
        public IReadOnlyDictionary<KeyCode, IAbilityReadOnly> KeyMap => _map;
        
        public event Action<IAbilityReadOnly> AbilityAdded;
        public event Action<IAbilityReadOnly> AbilityRemoved;

        public KeyAbilityPool() : this(new List<KeyCode> { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Q, KeyCode.E })
        {
            
        }
        
        public KeyAbilityPool(IEnumerable<KeyCode> keys)
        {
            _order = keys;
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            if (ability == null || _map.ContainsValue(ability)) return;
            
            var key = _order.FirstOrDefault(x => !_map.ContainsKey(x));
            
            if (key == KeyCode.None) Debug.LogWarning($"No free keys left for ability {ability}. Assigned KeyCode.None.");
            
            _map[key] = ability;
            AbilityAdded?.Invoke(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            if (ability == null) return;

            foreach (var kvp in _map)
            {
                if (kvp.Value != ability) continue;

                _map.Remove(kvp.Key);
                
                AbilityRemoved?.Invoke(kvp.Value);
                
                break;
            }
        }
        
        public bool Contains(IAbilityReadOnly ability) => _map.ContainsValue(ability);
    }
}