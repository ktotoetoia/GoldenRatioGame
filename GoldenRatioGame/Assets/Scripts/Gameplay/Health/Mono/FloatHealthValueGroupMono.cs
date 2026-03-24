using System;
using System.Collections.Generic;
using IM.Values;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthValueGroupMono : MonoBehaviour, IFloatHealthValuesGroup, IFloatHealthEvents
    {
        private readonly FloatHealthValuesGroup _floatHealthValueGroup = new();

        public event Action<float> OnHealthChanged
        {
            add => _floatHealthValueGroup.OnHealthChanged += value;
            remove => _floatHealthValueGroup.OnHealthChanged -= value;
        }

        public ICappedValueReadOnly<float> Health => _floatHealthValueGroup.Health;
        public IReadOnlyList<ICappedValueReadOnly<float>> Values => _floatHealthValueGroup.Values;
        
        public HealthChangeResult PreviewDamage(float incomingDamage)
        {
            return _floatHealthValueGroup.PreviewDamage(incomingDamage);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            return _floatHealthValueGroup.TakeDamage(damage);
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            return _floatHealthValueGroup.PreviewHealing(healing);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            return _floatHealthValueGroup.RestoreHealth(healing);
        }

        public void AddHealth(ICappedValue<float> healthBar)
        {
            _floatHealthValueGroup.AddHealth(healthBar);
        }

        public void RemoveHealth(ICappedValue<float> healthBar)
        {
            _floatHealthValueGroup.RemoveHealth(healthBar);
        }

        public bool Contains(ICappedValueReadOnly<float> healthBar)
        {
            return _floatHealthValueGroup.Contains(healthBar);
        }
    }
}