using System;
using IM.Values;

namespace IM.Abilities
{
    public class ChannelInfo : IChannelInfo
    {
        public event Action OnChannelFinished;
        public event Action OnChannelInterrupted;
        
        private bool _finishedOrInterrupted;
        
        public IChannelAbility Ability { get; }
        public UseContext UseContext { get; private set; }
        public bool Completed =>  _finishedOrInterrupted;
        
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

        public void UpdateAbilityUseContext(UseContext useContext)
        {
            UseContext = useContext;
        }
    }
}