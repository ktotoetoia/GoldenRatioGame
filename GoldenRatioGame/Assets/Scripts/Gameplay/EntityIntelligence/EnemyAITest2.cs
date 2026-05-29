using System.Collections.Generic;
using IM.Abilities;
using IM.Factions;
using IM.LifeCycle;
using IM.Map;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class EnemyAITest2 : MonoBehaviour
    {
        [Range(1,100)]
        [SerializeField] private float _closeDistance = 3f;
        [SerializeField] private CappedValue<float> _timeBetweenAttacks = new (1,4);
        private IFactionMemberReadOnly _factionMember;
        private PlayerStateMachine _playerStateMachine;
        private IEntity _currentTarget;
        private List<IEntity> _awareOf = new();
        private IRoomVisitor _roomVisitor;
        private float _nextUseTime;
        
        private void Awake()
        {
            _roomVisitor = GetComponent<IRoomVisitor>();
            _factionMember = GetComponent<IFactionMemberReadOnly>();
            _playerStateMachine = GetComponent<PlayerStateMachine>();
            _playerStateMachine.ProvideMovementDirection = ProvideMovementDirection;
            _playerStateMachine.ResolveRequestedAbilities = ResolveRequestedAbilities;
        }

        private void OnEnable()
        {
            foreach (ModuleEntity entity in FindObjectsByType<ModuleEntity>())
            {
                if (entity.TryGetComponent(out IFactionMemberReadOnly factionMember))
                {
                    _awareOf.Add(entity);
                }
            }
        }

        private IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>> ResolveRequestedAbilities(IEnumerable<IAbilityReadOnly> x)
        {
            if (_currentTarget == null) return new List<KeyValuePair<IAbilityReadOnly, UseContext>>();
            
            Dictionary<IAbilityReadOnly,UseContext> result = new();
            UseContext context = new UseContext(_currentTarget.GameObject.transform.position,transform.position);

            if (Time.time > _nextUseTime)
            {
                foreach (IAbilityReadOnly ability in x)
                {
                    result[ability] = context;
                }
                
                _nextUseTime = Time.time +Random.Range(_timeBetweenAttacks.MinValue,_timeBetweenAttacks.MaxValue);
            }
            
            return result;
        }

        private Vector2 ProvideMovementDirection() => Vector2.zero;

        private void Update()
        {
            foreach (IEntity entity in _awareOf)
            {
                if (!entity.IsDestroyed && entity.GameObject.TryGetComponent(out IFactionMemberReadOnly factionMemberReadOnly) && _factionMember.Faction.GetRelationWith(factionMemberReadOnly.Faction) == FactionRelation.Enemy)
                {
                    _currentTarget =  entity;
                }
            }
        }
    }
}