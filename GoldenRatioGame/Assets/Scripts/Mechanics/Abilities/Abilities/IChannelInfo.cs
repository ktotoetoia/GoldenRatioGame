using System;

namespace IM.Abilities
{
    public interface IChannelInfo
    {
        IChannelAbility Ability { get; }
        
        event Action OnChannelFinished;
        event Action OnChannelInterrupted;
        
        public void UpdateAbilityUseContext(AbilityUseContext abilityUseContext);
    }
}