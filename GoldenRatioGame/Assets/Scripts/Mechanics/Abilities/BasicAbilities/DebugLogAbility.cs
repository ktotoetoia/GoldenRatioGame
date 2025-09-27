using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class DebugLogAbility : IAbility, IPreferredKeyboardBinding
    {
        private readonly string _textToLog;
        

        public KeyCode Key { get; set; } = KeyCode.G;
        public ICooldownReadOnly Cooldown { get; }
        public bool IsBeingUsed => false;
        public bool CanUse => !Cooldown.IsOnCooldown;

        public DebugLogAbility() : this("ability has been used", new FloatCooldown(0))
        {
            
        }

        public DebugLogAbility(string textToLog, ICooldown cooldown)
        {
            _textToLog = textToLog;
            Cooldown = cooldown;
        }
        
        
        public bool TryUse()
        {
            if (CanUse)
            {
                Debug.Log(_textToLog);
                
                return true;
            }

            return false;
        }
    }
}