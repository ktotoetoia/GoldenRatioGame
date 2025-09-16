using System.Collections.Generic;
using IM.Values;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthComponentsGroupMono : MonoBehaviour, IFloatHealthComponentsGroup
    {
        private readonly IFloatHealthComponentsGroup _floatHealth= new FloatHealthComponentsGroup();

        public ICappedValueReadOnly<float> Health => _floatHealth.Health;
        public IReadOnlyList<IFloatHealth> Components => _floatHealth.Components;

        public HealthChangeResult PreviewDamage(float incomingDamage)
        {
            return _floatHealth.PreviewDamage(incomingDamage);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            return _floatHealth.TakeDamage(damage);
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            return _floatHealth.PreviewHealing(healing);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            return _floatHealth.RestoreHealth(healing);
        }

        public void AddHealth(IFloatHealth healthComponent)
        {
            _floatHealth.AddHealth(healthComponent);
        }

        public void RemoveHealth(IFloatHealth healthComponent)
        {
            _floatHealth.RemoveHealth(healthComponent);
        }
    }
}