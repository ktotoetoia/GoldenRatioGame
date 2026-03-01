using System;
using System.Collections.Generic;
using IM.Effects;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPoolUserMono : MonoBehaviour, IAbilityUser<IKeyAbilityPool>, IAbilityUserEvents
    {
        private IEffectContainer _effectContainer;
        private IAbilityUseInfo _abilityUseInfo;
        private IAbilityReadOnly _currentAbility;
        private IEffectGroup _appliedEffectGroup;
        
        public event Action<IAbilityReadOnly> OnAbilityStarted;
        public event Action<IAbilityReadOnly> OnAbilityFinished;
        
        public IKeyAbilityPool AbilityPool { get; private set; }
        public Func<IAbilityReadOnly, AbilityUseContext> GetAbilityUseContext { get; set; } =
            x => new AbilityUseContext();
        
        private IAbilityReadOnly CurrentAbility
        {
            get => _currentAbility;
            set
            {
                IAbilityReadOnly previous = _currentAbility;
                _currentAbility = value;
                
                if (previous != null)
                {
                    if(previous is IApplyEffectGroupOnUse) _effectContainer.RemoveGroup(_appliedEffectGroup);
                    
                    OnAbilityFinished?.Invoke(previous);
                }
                if (_currentAbility != null)
                {
                    if(_currentAbility is IApplyEffectGroupOnUse effectGroupOnUse) _effectContainer.AddGroup(_appliedEffectGroup = effectGroupOnUse.GetEffectGroup());
                    
                    OnAbilityStarted?.Invoke(_currentAbility);
                }
            }
        }
        
        private void Awake()
        {
            _effectContainer = GetComponent<IEffectContainer>();
            AbilityPool = GetComponent<IKeyAbilityPool>();
        }
        
        private void Update()
        {
            if(CurrentAbility == null) return;
            if (_abilityUseInfo.Completed) CurrentAbility = null;
            
            if (CurrentAbility is IChannelAbility && _abilityUseInfo is IChannelInfo channelInfo)
            {
                channelInfo.UpdateAbilityUseContext(GetAbilityUseContext(channelInfo.Ability));
            }
        }
        
        public void ResolveRequestedAbilities(IEnumerable<IAbilityReadOnly> requestedAbilities, AbilityUseContext abilityUseContext)
        {
            HashSet<IAbilityReadOnly> requested = requestedAbilities as HashSet<IAbilityReadOnly> 
                                                  ?? new HashSet<IAbilityReadOnly>(requestedAbilities);
            
            if (CurrentAbility != null)
            {
                if (_abilityUseInfo.Completed)
                {
                    CurrentAbility = null;
                }
                else
                {
                    bool stillRequested = requested.Contains(CurrentAbility);

                    if (!stillRequested && CurrentAbility is IInterruptable interruptable && interruptable.TryInterrupt())
                    {
                        CurrentAbility = null;
                    }
                    else if (CurrentAbility is IChannelAbility && _abilityUseInfo is IChannelInfo channelInfo)
                    {
                        channelInfo.UpdateAbilityUseContext(GetAbilityUseContext(CurrentAbility));
                    }
                }
            }
            
            if (CurrentAbility != null) return;
            
            foreach (var ability in requested)
            {
                if (TryStartAbility(ability, abilityUseContext)) break;
            }
        }
        
        private bool TryStartAbility(IAbilityReadOnly ability, AbilityUseContext ctx)
        {
            if (ability == null || !AbilityPool.Contains(ability) || !ability.CanUse) return false;

            if (ability is IRequireAbilityUseContext contextual) contextual.UpdateAbilityUseContext(ctx);

            if (ability is ICastAbility cast && cast.TryCast(out var castInfo))
            {
                _abilityUseInfo = castInfo;
                CurrentAbility = cast;
                return true;
            }

            if (ability is IChannelAbility channel && channel.TryChannel(out var channelInfo))
            {
                _abilityUseInfo = channelInfo;
                CurrentAbility = channel;
                return true;
            }

            return false;
        }
    }
}