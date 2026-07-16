using System;
using System.Collections.Generic;
using IM.Values;
using UnityEngine;

namespace IM.Health
{
    public class FloatHealthValueGroupMono : MonoBehaviour, IFloatHealthValuesGroup, IFloatHealthEvents
    {
        [SerializeField] private bool _useDivided;


        private IFloatHealthValuesGroup _floatHealthValuesGroup;
        private  IFloatHealthValuesGroup FloatHealthValueGroup => _floatHealthValuesGroup ??= _useDivided ? new DividedFloatHealthValuesGroup(): new FloatHealthValuesGroup();

        public event Action<float> OnHealthChanged
        {
            add => ((IFloatHealthEvents)FloatHealthValueGroup).OnHealthChanged += value;
            remove => ((IFloatHealthEvents)FloatHealthValueGroup).OnHealthChanged -= value;
        }
        
        public ICappedValueReadOnly<float> Health => FloatHealthValueGroup.Health;
        public IReadOnlyList<ICappedValueReadOnly<float>> Values => FloatHealthValueGroup.Values;
        
        public HealthChangeResult PreviewDamage(float incomingDamage)
        {
            return FloatHealthValueGroup.PreviewDamage(incomingDamage);
        }

        public HealthChangeResult TakeDamage(float damage)
        {
            return FloatHealthValueGroup.TakeDamage(damage);
        }

        public HealthChangeResult PreviewHealing(float healing)
        {
            return FloatHealthValueGroup.PreviewHealing(healing);
        }

        public HealthChangeResult RestoreHealth(float healing)
        {
            return FloatHealthValueGroup.RestoreHealth(healing);
        }

        public void AddHealth(ICappedValue<float> healthBar)
        {
            FloatHealthValueGroup.AddHealth(healthBar);
        }

        public void RemoveHealth(ICappedValue<float> healthBar)
        {
            FloatHealthValueGroup.RemoveHealth(healthBar);
        }

        public bool Contains(ICappedValueReadOnly<float> healthBar)
        {
            return FloatHealthValueGroup.Contains(healthBar);
        }
    }
}