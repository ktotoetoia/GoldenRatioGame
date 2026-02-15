using IM.Abilities;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class DebugLogAbilityExtension : MonoBehaviour, IAbilityExtension
    {
        [SerializeField] private string _textToLog;
        [SerializeField] private int _cooldown;
        
        public IAbility Ability { get; private set; }

        private void Awake()
        {
            Ability = new DebugLogAbility(_textToLog, new FloatCooldown(_cooldown)){};
        }
    }
}