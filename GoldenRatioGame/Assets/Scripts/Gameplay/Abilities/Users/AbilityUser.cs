using System;
using System.Collections.Generic;
using IM.Effects;
using IM.Values;

namespace IM.Abilities
{
    public class AbilityUser : IAbilityUser<IAbilityPoolReadOnly>, IAbilityUserEvents
    {
        private readonly IEffectContainer _effectContainer;
        private IAbilityUseInfo _abilityUseInfo;
        private IAbilityReadOnly _currentAbility;
        private IEffectGroup _appliedEffectGroup;
        
        public event Action<IAbilityReadOnly> OnAbilityStarted;
        public event Action<IAbilityReadOnly> OnAbilityFinished;
        
        public IAbilityPoolReadOnly AbilityPool { get; }
        
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
                    if(_currentAbility is IApplyEffectGroupOnUse effectGroupOnUse) 
                        _effectContainer.AddGroup(_appliedEffectGroup = effectGroupOnUse.GetEffectGroup());
                    
                    OnAbilityStarted?.Invoke(_currentAbility);
                }
            }
        }
        
        public AbilityUser(IAbilityPoolReadOnly abilityPool, IEffectContainer effectContainer)
        {
            AbilityPool = abilityPool;
            _effectContainer = effectContainer;
        }
        
        public void ResolveRequestedAbilities(IEnumerable<IAbilityReadOnly> requestedAbilities, UseContext useContext)
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
                        channelInfo.UpdateAbilityUseContext(useContext);
                    }
                }
            }
            
            if (CurrentAbility != null) return;
            
            foreach (var ability in requested)
            {
                if (TryStartAbility(ability, useContext)) break;
            }
        }
        
        private bool TryStartAbility(IAbilityReadOnly ability, UseContext ctx)
        {
            if (ability == null || !AbilityPool.Contains(ability) || !ability.CanUse) return false;

            if (ability is IRequireAbilityUseContext contextual) contextual.UpdateAbilityUseContext(ctx);

            if (ability is ICastAbility cast && cast.TryCast(out var castInfo))
            {
                _abilityUseInfo = castInfo;
                CurrentAbility = cast;
                return true;
            }

            if (ability is not IChannelAbility channel || !channel.TryChannel(out var channelInfo)) return false;
            
            _abilityUseInfo = channelInfo;
            CurrentAbility = channel;
            return true;
        }   
    }
}