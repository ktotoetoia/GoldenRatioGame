using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class DebugDrawLineChannelAbility : IChannelAbility
    {
        private readonly ICooldown _cooldown;
        private ChannelInfo _channelInfo;
        private readonly ICooldown _channelCooldown;
        
        public ICooldownReadOnly Cooldown=> _cooldown;
        public bool CanUse => !Cooldown.IsOnCooldown;
        public bool IsChanneling => _channelInfo != null;

        public DebugDrawLineChannelAbility(float _cooldownTime, float _useTime )
        {
            _cooldown = new FloatCooldown(_cooldownTime);
            _channelCooldown =  new FloatCooldown(_useTime);
        }

        public void Update()
        {
            if(!IsChanneling) return;
            Debug.Log("channeling");
            
            Debug.DrawLine(_channelInfo.AbilityUseContext.EntityPosition,_channelInfo.AbilityUseContext.TargetWorldPosition);
            
            
            if (!_channelCooldown.IsOnCooldown)
            {
                Debug.Log("interrupted");
                _channelInfo.CallOnChannelFinished();
                _channelInfo =  null;
            }
        }

        public bool TryChannel(out IChannelInfo channelInfo)
        {
            if (IsChanneling)
            {
                channelInfo = null;
                return false;
            }
            
            _channelInfo = new ChannelInfo(this);
            channelInfo = _channelInfo;
            _channelCooldown.ForceReset();
            
            return true;
        }

        public void Interrupt()
        {
            _channelInfo.CallOnChannelInterrupt();
            _channelInfo = null;
        }
    }
}