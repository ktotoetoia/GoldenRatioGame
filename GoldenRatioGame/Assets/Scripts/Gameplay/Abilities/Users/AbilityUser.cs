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
        private bool _isInterrupted;
        
        public event Action<IAbilityReadOnly> AbilityStarted;
        public event Action<IAbilityReadOnly> AbilityFinished;
        
        public IAbilityPoolReadOnly AbilityPool { get; }
        public bool IsInterrupted 
        { 
            get => _isInterrupted;
            set
            {
                if (_isInterrupted == value) return;
                _isInterrupted = value;

                if (_isInterrupted) InterruptCurrentAbility();
            }
        }
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
                    
                    AbilityFinished?.Invoke(previous);
                }
                if (_currentAbility != null)
                {
                    if(_currentAbility is IApplyEffectGroupOnUse effectGroupOnUse) 
                        _effectContainer.AddGroup(_appliedEffectGroup = effectGroupOnUse.GetEffectGroup());
                    
                    AbilityStarted?.Invoke(_currentAbility);
                }
            }
        }
        
        public AbilityUser(IAbilityPoolReadOnly abilityPool, IEffectContainer effectContainer)
        {
            AbilityPool = abilityPool;
            _effectContainer = effectContainer;
        }
        
        public void ResolveRequestedAbilities(IEnumerable<KeyValuePair<IAbilityReadOnly,UseContext>> requestedAbilities)
        {
            if (IsInterrupted) return;
            
            Dictionary<IAbilityReadOnly, UseContext> requested = new(requestedAbilities);
            
            if (CurrentAbility != null)
            {
                if (_abilityUseInfo.Completed)
                {
                    CurrentAbility = null;
                }
                else
                {
                    
                    bool stillRequested = requested.TryGetValue(CurrentAbility,out UseContext context);

                    if (!stillRequested && CurrentAbility is IInterruptable interruptable && interruptable.TryInterrupt())
                    {
                        CurrentAbility = null;
                    }
                    else if (CurrentAbility is IChannelAbility && _abilityUseInfo is IChannelInfo channelInfo)
                    {
                        channelInfo.UpdateAbilityUseContext(context);
                    }
                }
            }
            
            if (CurrentAbility != null) return;
            
            foreach ((IAbilityReadOnly ability, UseContext context) in requested)
            {
                if (TryStartAbility(ability, context)) break;
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
        private void InterruptCurrentAbility()
        {
            if (CurrentAbility == null) return;

            if (_abilityUseInfo is { Completed: true })
            {
                CurrentAbility = null;
                return;
            }

            if (CurrentAbility is not IInterruptable interruptable) return;
            
            if (interruptable.TryInterrupt())
            {
                CurrentAbility = null;
            }
        }
    }
}