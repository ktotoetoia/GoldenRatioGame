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
            _moduleEditingContextEditor = GetComponent<IModuleEditingContextEditor>();
            _abilityUser = new AbilityUser(new ReferenceAbilityPoolReadOnly(() => _moduleEditingContextEditor.Snapshot.Capabilities.Get<IAbilityPoolReadOnly>()), GetComponent<IEffectContainer>());
        }
        
        public void ResolveRequestedAbilities(IEnumerable<KeyValuePair<IAbilityReadOnly, UseContext>> requestedAbilities)
        {
            _abilityUser.ResolveRequestedAbilities(requestedAbilities);
        }
    }
}