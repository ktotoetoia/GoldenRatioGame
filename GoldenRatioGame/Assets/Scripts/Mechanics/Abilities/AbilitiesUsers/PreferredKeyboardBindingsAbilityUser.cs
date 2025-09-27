using UnityEngine;

namespace IM.Abilities
{
    public class PreferredKeyboardBindingsAbilityUser
    {
        public IAbilityPool Pool { get; set; }
        
        public PreferredKeyboardBindingsAbilityUser(IAbilityPool pool)
        {
            Pool = pool;
        }

        public void Update()
        {
            foreach (IAbility ability in Pool.Abilities)
            {
                if (ability is IPreferredKeyboardBinding preferred && Input.GetKeyDown(preferred.Key))
                {
                    ability.TryUse();
                }
            }
        }
    }
}