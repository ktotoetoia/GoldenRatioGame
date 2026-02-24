using System;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPoolUserMono : MonoBehaviour, IAbilityUser<IKeyAbilityPool>, IAbilityUserEvents
    {
        private IChannelInfo _channelInfo;
        public IKeyAbilityPool AbilityPool { get;private set; }
        public event Action<IAbilityReadOnly, AbilityUseContext> OnAbilityUsed;

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
        
        public void UseAbility(IAbilityReadOnly ability)
        {            
            if(!CanUseAbility(ability)) return;
            if (ability == null) throw new ArgumentNullException(nameof(ability));
            if(!AbilityPool.Contains(ability)) throw new ArgumentException($"Ability {ability} does not exist in the key pool.");

            if (ability.CanUse && ability is IUseContextAbility c)
            {
                c.UpdateAbilityUseContext(GetAbilityUseContext(ability));
            }

            if(ability is IInstantAbility i && i.TryUse())
            {
                OnAbilityUsed?.Invoke(ability,GetAbilityUseContext(ability));
                
                return;
            }

            if (ability is IChannelAbility ch && ch.TryChannel(out _channelInfo))
            {
                _channelInfo.OnChannelFinished += () =>
                {
                    Debug.Log("sfasdf");
                    _channelInfo = null;
                };
                _channelInfo.OnChannelInterrupted += () =>
                {
                    Debug.Log("sfasdf");
                    _channelInfo = null;
                };
                OnAbilityUsed?.Invoke(ability,GetAbilityUseContext(ability));
            }
        }
    }
}