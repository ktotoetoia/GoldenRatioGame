using System;
using System.Collections.Generic;
using IM.Effects;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilityUserMono : MonoBehaviour, IAbilityUser<IAbilityPoolReadOnly>, IAbilityUserEvents
    {
        private AbilityUser _abilityUser;
        public IAbilityPoolReadOnly AbilityPool => _abilityUser.AbilityPool;

        public event Action<IAbilityReadOnly> OnAbilityStarted
        {
            add => _abilityUser.OnAbilityStarted += value;
            remove => _abilityUser.OnAbilityStarted -= value;
        }

        public event Action<IAbilityReadOnly> OnAbilityFinished
        {
            add => _abilityUser.OnAbilityFinished += value;
            remove => _abilityUser.OnAbilityFinished -= value;
        }

        private void Awake()
        {
            _abilityUser = new AbilityUser(GetComponent<IAbilityPoolReadOnly>(), GetComponent<IEffectContainer>());
        }
        
        public void ResolveRequestedAbilities(IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>> requestedAbilities)
        {
            _abilityUser.ResolveRequestedAbilities(requestedAbilities);
        }
    }
}