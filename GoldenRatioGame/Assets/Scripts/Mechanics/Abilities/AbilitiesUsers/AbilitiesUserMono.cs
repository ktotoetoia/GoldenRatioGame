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
            foreach (IAbility ability in _pool.Abilities)
            {
                if (ability is IPreferredKeyboardBinding preferred && Input.GetKeyDown(preferred.Key))
                {
                    ability.TryUse();
                }
            }
        }
    }
}