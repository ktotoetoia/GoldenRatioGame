using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class DebugLogAbility : IAbility
    {
        private readonly string _textToLog;
        private readonly ICooldown _cooldown;
        
        public ICooldownReadOnly Cooldown=> _cooldown;
        public bool IsBeingUsed => false;
        public bool CanUse => !Cooldown.IsOnCooldown;

        public DebugLogAbility() : this("ability has been used", new FloatCooldown(0))
        {
            
        }

        public DebugLogAbility(string textToLog, ICooldown cooldown)
        {
            _textToLog = textToLog;
            _cooldown = cooldown;
        }
        
        public bool TryUse()
        {
            if (!_cooldown.TryReset()) return false;
            
            Debug.Log(_textToLog);
         
            return true;
        }
    }
}