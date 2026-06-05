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

        public event Action<IAbilityReadOnly> AbilityStarted
        {
            add => _abilityUser.AbilityStarted += value;
            remove => _abilityUser.AbilityStarted -= value;
        }

        public event Action<IAbilityReadOnly> AbilityFinished
        {
            add => _abilityUser.AbilityFinished += value;
            remove => _abilityUser.AbilityFinished -= value;
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