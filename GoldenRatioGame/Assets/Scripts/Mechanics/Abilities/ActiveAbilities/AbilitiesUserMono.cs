using System.Linq;
using UnityEngine;

namespace IM.Abilities
{
    public class AbilitiesUserMono : MonoBehaviour
    {
        private IAbilitiesPool _pool;

        private void Awake()
        {
            _pool = GetComponent<IAbilitiesPool>();
        }

        private void Update()
        {
            foreach (IActiveAbility ability in _pool.ActiveAbilities)
            {
                if (ability is IPreferredKeyboardBinding preferred && Input.GetKeyDown(preferred.Key) && ability.TryUse() )
                {
                    
                }
            }
        }
    }
}