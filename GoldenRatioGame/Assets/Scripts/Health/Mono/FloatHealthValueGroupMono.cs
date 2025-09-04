using System.Collections.Generic;
using IM.Economy;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthValueGroupMono : MonoBehaviour, IFloatHealthValueGroup
    {
        private readonly IFloatHealthValueGroup _floatHealthValueGroupImplementation = new FloatHealthValueGroup();

        public ICappedValueReadOnly<float> Health => _floatHealthValueGroupImplementation.Health;
        public IReadOnlyList<ICappedValueReadOnly<float>> HealthBars => _floatHealthValueGroupImplementation.HealthBars;
        
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
    }
}