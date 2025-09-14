using System.Collections.Generic;
using IM.Economy;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthValueGroupMono : MonoBehaviour, IFloatHealthValuesGroup
    {
        private readonly IFloatHealthValuesGroup _floatHealthValueGroupImplementation = new FloatHealthValuesGroup();

        public ICappedValueReadOnly<float> Health => _floatHealthValueGroupImplementation.Health;
        public IReadOnlyList<ICappedValueReadOnly<float>> Values => _floatHealthValueGroupImplementation.Values;
        
        public HealthChangeResult PreviewDamage(float incomingDamage)
        {
            return _floatHealthValueGroupImplementation.PreviewDamage(incomingDamage);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            return _floatHealthValueGroupImplementation.TakeDamage(damage);
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            return _floatHealthValueGroupImplementation.PreviewHealing(healing);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            return _floatHealthValueGroupImplementation.RestoreHealth(healing);
        }

        public void AddHealth(ICappedValue<float> healthBar)
        {
            _floatHealthValueGroupImplementation.AddHealth(healthBar);
        }

        public void RemoveHealth(ICappedValue<float> healthBar)
        {
            _floatHealthValueGroupImplementation.RemoveHealth(healthBar);
        }

        public bool Contains(ICappedValueReadOnly<float> healthBar)
        {
            return _floatHealthValueGroupImplementation.Contains(healthBar);
        }
    }
}