using IM.Abilities;
using IM.LifeCycle;
using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    public class ApplyEffectsOnAbilityUsed : MonoBehaviour
    {
        [SerializeField] private EffectGroupFactory _effectGroupFactory;
        private IAbilityEvents _abilityEvents;
        private IRequireEntity _entitySource;
        
        private IEntity _lastEntity;
        private IEffectContainer _effectContainer;
        
        private void Awake()
        {
            _entitySource = GetComponent<IRequireEntity>();
            _abilityEvents = GetComponent<IAbilityEvents>();
            _abilityEvents.AbilityFired += Apply;
        }

        private void Apply(UseContext context)
        {
            UpdateEntity();

            IEffectContext effectContext = new EffectContext(gameObject, _entitySource.Entity.GameObject);

            _effectContainer?.AddGroup(_effectGroupFactory.Create(effectContext));
        }

        private void UpdateEntity()
        {
            if (_lastEntity == _entitySource.Entity) return;
            
            _lastEntity = _entitySource.Entity;
            _lastEntity?.GameObject.TryGetComponent(out _effectContainer);
        }

        private void OnDestroy()
        {
            if(_abilityEvents == null) return;
            
            _abilityEvents.AbilityFired -= Apply;
        }
    }
}