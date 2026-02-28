using System.Collections.Generic;
using IM.Modules;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class DebugDrawLineChannelAbility : IChannelAbility, IInterruptable
    {
        private readonly ICooldown _cooldown;
        private readonly ICooldown _channelCooldown;
        private ChannelInfo _channelInfo;
        
        public ICooldownReadOnly Cooldown=> _cooldown;
        public bool CanUse => !Cooldown.IsOnCooldown;
        public bool IsChanneling => _channelInfo != null;
        public bool CanInterrupt => true;

        public ITypeRegistry<IAbilityDescriptor> AbilityDescriptorsRegistry { get; set; } =
            new TypeRegistry<IAbilityDescriptor>(new List<IAbilityDescriptor> { new BlockUserMovementAbility{BlockUserMovement = true} });
        
        public DebugDrawLineChannelAbility(float cooldownTime, float useTime)
        {
            _cooldown = new FloatCooldown(cooldownTime);
            _channelCooldown =  new FloatCooldown(useTime);
        }

        public void Update()
        {
            if(!IsChanneling) return;
            
            Debug.DrawLine(_channelInfo.AbilityUseContext.EntityPosition,_channelInfo.AbilityUseContext.TargetWorldPosition);
            
            if (!_channelCooldown.IsOnCooldown) Interrupt();    
        }

        public bool TryChannel(out IChannelInfo channelInfo)
        {
            if (IsChanneling || _cooldown.IsOnCooldown)
            {
                channelInfo = null;
                return false;
            }
            
            _channelInfo = new ChannelInfo(this);
            channelInfo = _channelInfo;
            _channelCooldown.ForceReset();
            _cooldown.ForceReset();
            
            return true;
        }


        public bool TryInterrupt()
        {
            if (CanInterrupt)
            {
                Interrupt();
                return true;
            }

            return false;
        }

        public void Interrupt()
        {
            _channelInfo.CallOnChannelInterrupt();
            _channelInfo = null;
        }
    }
}