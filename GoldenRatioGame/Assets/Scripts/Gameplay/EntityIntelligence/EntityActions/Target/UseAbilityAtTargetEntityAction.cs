using System.Collections.Generic;
using IM.Abilities;
using IM.Values;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class UseAbilityAtTargetEntityAction : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IAbilityUser<IAbilityPoolReadOnly> _abilityUser;
        private readonly TargetMemory _targetMemory;
        
        private readonly float _minCooldown;
        private readonly float _maxCooldown;
        
        private float _nextAllowedCastTime;

        private readonly Dictionary<IAbilityReadOnly, UseContext> _requestedAbilities = new();

        public UseAbilityAtTargetEntityAction(
            Transform ownerTransform, 
            IAbilityUser<IAbilityPoolReadOnly> abilityUser, 
            IMemoryContainer memoryContainer,
            float minCooldown,
            float maxCooldown)
        {
            _ownerTransform = ownerTransform;
            _abilityUser    = abilityUser;
            _minCooldown    = minCooldown;
            _maxCooldown    = maxCooldown;

            if (!memoryContainer.Memories.TryGet(out _targetMemory))
            {
                _targetMemory = new TargetMemory();
                memoryContainer.Add(_targetMemory);
            }

            _nextAllowedCastTime = Time.time;
        }

        public override void Update()
        {
            if (!_targetMemory.Target) return;

            if (Time.time < _nextAllowedCastTime) return;

            _requestedAbilities.Clear();

            UseContext context = new UseContext(_targetMemory.Target.transform.position, _ownerTransform.position);

            foreach (IAbilityReadOnly ability in _abilityUser.AbilityPool)
            {
                if (ability.CanUse) _requestedAbilities.Add(ability, context);
            }

            if (_requestedAbilities.Count == 0) return;

            _abilityUser.ResolveRequestedAbilities(_requestedAbilities);

            CalculateNextCooldown();
        }

        private void CalculateNextCooldown()
        {
            float randomCooldown = Random.Range(_minCooldown, _maxCooldown);
            _nextAllowedCastTime = Time.time + randomCooldown;
        }
    }
}