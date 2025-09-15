using IM.Economy;
using IM.Entities;
using IM.Health;

namespace IM.Modules
{
    public class HealthHolder : IEntityHolder
    {
        private readonly CappedValue<float> _health;
        private IFloatHealthValuesGroup _healthValuesGroup;
        
        private IEntity _entity;
        public IEntity Entity
        {
            get => _entity;
            set
            {
                if (_entity != null)
                {
                    _healthValuesGroup?.RemoveHealth(_health);
                }
                
                _entity = value;

                if (value != null && value.GameObject.TryGetComponent(out _healthValuesGroup))
                {
                    _healthValuesGroup.AddHealth(_health);
                }
            }
        }

        public HealthHolder(float maxHealth, float currentHealth)
        {
            _health = new CappedValue<float>(0,maxHealth,currentHealth);
        }
    }
}