using UnityEngine;

namespace IM.Abilities
{
    public class AbilitiesUserMono : MonoBehaviour
    {
        public IAbilitiesPool Pool { get; set; }

        private void Awake()
        {
            Pool = GetComponent<IAbilitiesPool>();
        }

        private void Update()
        {
            if(Pool == null) return;
            
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