using System;

namespace IM.Abilities
{
    public interface IChannelInfo
    {
        event Action OnChannelFinished;
        event Action OnChannelInterrupted;
        
        IChannelAbility Ability { get; }
        
        public void UpdateAbilityUseContext(AbilityUseContext abilityUseContext);
    }
}