using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityKeyPoolMono : MonoBehaviour, IKeyAbilityPool, IAbilityPool
    {
        [SerializeField] private List<KeyCode> _keys = new()
        {
            KeyCode.Q,
            KeyCode.E,
            KeyCode.R,
            KeyCode.Z,
            KeyCode.X,
        };

        private readonly Dictionary<KeyCode, IAbility> _map = new();
        private Queue<KeyCode> _freeKeys;

        public IReadOnlyCollection<IAbility> Abilities => _map.Values;
        public IReadOnlyDictionary<KeyCode, IAbility> KeyMap => _map;

        private void Awake()
        {
            _freeKeys = new Queue<KeyCode>(_keys);
        }

        public void AddAbility(IAbility ability)
        {
            if (ability == null || _map.ContainsValue(ability))
                return;

            var key = _freeKeys.Count > 0
                ? _freeKeys.Dequeue()
                : KeyCode.None;

            if (key == KeyCode.None)
                Debug.LogWarning($"No free keys left for ability {ability}. Assigned KeyCode.None.");

            _map[key] = ability;
        }

        public void RemoveAbility(IAbility ability)
        {
            if (ability == null)
                return;

            foreach (var kvp in _map)
            {
                if (kvp.Value != ability)
                    continue;

                _map.Remove(kvp.Key);

                if (kvp.Key != KeyCode.None)
                    _freeKeys.Enqueue(kvp.Key);

                break;
            }
        }
    }
}