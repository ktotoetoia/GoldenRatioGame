using System;
using System.Collections.Generic;
using IM.Abilities;
using IM.Effects;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class ContextSnapshotAbilityUser : MonoBehaviour, IAbilityUser<IAbilityPoolReadOnly>, IAbilityUserEvents
    {
        private AbilityUser _abilityUser;
        private IModuleEditingContextEditor _moduleEditingContextEditor;
        
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
            _moduleEditingContextEditor = GetComponent<IModuleEditingContextEditor>();
            _abilityUser = new AbilityUser(new ReferenceAbilityPoolReadOnly(() => _moduleEditingContextEditor.Snapshot.Capabilities.Get<IAbilityPoolReadOnly>()), GetComponent<IEffectContainer>());
        }
        
        public void ResolveRequestedAbilities(IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>> requestedAbilities)
        {
            _abilityUser.ResolveRequestedAbilities(requestedAbilities);
        }
    }
}