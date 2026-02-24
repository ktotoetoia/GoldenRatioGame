using System;

namespace IM.Abilities
{
    public class ChannelInfo : IChannelInfo
    {
        public event Action OnChannelFinished;
        public event Action OnChannelInterrupted;
        
        private bool _finishedOrInterrupted;
        
        public IChannelAbility Ability { get; }
        public AbilityUseContext AbilityUseContext { get; private set; }
        
        public ChannelInfo(IChannelAbility ability)
        {
            Ability = ability;
        }

        public void CallOnChannelFinished()
        {
            if (_finishedOrInterrupted) throw new InvalidOperationException("this channel was already finished or interrupted");
            
            OnChannelFinished?.Invoke();
            _finishedOrInterrupted = true;
        }

        public void CallOnChannelInterrupt()
        {
            if (_finishedOrInterrupted) throw new InvalidOperationException("this channel was already finished or interrupted");

            OnChannelInterrupted?.Invoke();
            _finishedOrInterrupted = true;
        }

        public void UpdateAbilityUseContext(AbilityUseContext abilityUseContext)
        {
            AbilityUseContext = abilityUseContext;
        }
    }
}