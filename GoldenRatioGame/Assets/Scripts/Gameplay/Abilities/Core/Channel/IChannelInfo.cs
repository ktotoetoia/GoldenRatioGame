using System;
using IM.Values;

namespace IM.Abilities
{
    public interface IChannelInfo : IAbilityUseInfo
    {
        event Action OnChannelFinished;
        event Action OnChannelInterrupted;
        
        IChannelAbility Ability { get; }
        
        public void UpdateAbilityUseContext(UseContext useContext);
    }
}