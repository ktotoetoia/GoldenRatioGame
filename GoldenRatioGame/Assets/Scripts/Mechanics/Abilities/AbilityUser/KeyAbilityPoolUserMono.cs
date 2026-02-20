using System;
using UnityEngine;

namespace IM.Abilities
{
    public class KeyAbilityPoolUserMono : MonoBehaviour, IAbilityUser<IKeyAbilityPool>, IAbilityUserEvents
    {
        public IKeyAbilityPool AbilityPool { get;private set; }
        public event Action<IAbility, AbilityUseContext> OnAbilityUsed;
        
        private void Awake()
        {
            AbilityPool = GetComponent<IKeyAbilityPool>();
        }

        public void UseAbility(IAbility ability, AbilityUseContext useContext)
        {
            if (ability == null) throw new ArgumentNullException(nameof(ability));
            if(!AbilityPool.Contains(ability)) throw new ArgumentException($"Ability {ability} does not exist in the key pool.");

            if (ability.CanUse && ability is IUseContextAbility c)
            {
                c.UpdateAbilityUseContext(useContext);
            }

            if(ability.TryUse())
            {
                OnAbilityUsed?.Invoke(ability,useContext);
            }
        }
    }
}