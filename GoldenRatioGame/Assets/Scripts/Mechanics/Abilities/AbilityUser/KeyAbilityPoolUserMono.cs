using System;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPoolUserMono : MonoBehaviour, IAbilityUser<IKeyAbilityPool>, IAbilityUserEvents
    {
        private IChannelInfo _channelInfo;
        public event Action<IAbilityReadOnly, AbilityUseContext> OnAbilityUsed;
        
        public IKeyAbilityPool AbilityPool { get;private set; }
        public Func<IAbilityReadOnly, AbilityUseContext> GetAbilityUseContext { get; set; } =
            x => new AbilityUseContext();
        
        private void Awake()
        {
            AbilityPool = GetComponent<IKeyAbilityPool>();
        }
        
        private void Update()
        {
            _channelInfo?.UpdateAbilityUseContext(GetAbilityUseContext(_channelInfo.Ability));
        }

        public bool CanUseAbility(IAbilityReadOnly ability)
        {
            return ability.CanUse && _channelInfo == null;
        }
        
        public bool TryUseAbility(IAbilityReadOnly ability)
        {            
            if(!CanUseAbility(ability)) return false;
            if (ability == null) throw new ArgumentNullException(nameof(ability));
            if(!AbilityPool.Contains(ability)) throw new ArgumentException($"Ability {ability} does not exist in the key pool.");
            
            if (ability.CanUse && ability is IUseContextAbility c)
            {
                c.UpdateAbilityUseContext(GetAbilityUseContext(ability));
            }

            if(ability is ICastAbility i && i.TryCast(out _))
            {
                OnAbilityUsed?.Invoke(ability,GetAbilityUseContext(ability));
                return true;
            }

            if (ability is IChannelAbility ch && ch.TryChannel(out _channelInfo))
            {
                _channelInfo.OnChannelFinished += () =>
                {
                    _channelInfo = null;
                };
                _channelInfo.OnChannelInterrupted += () =>
                {
                    _channelInfo = null;
                };
                OnAbilityUsed?.Invoke(ability,GetAbilityUseContext(ability));
                
                return true;
            }

            return false;
        }
    }
}