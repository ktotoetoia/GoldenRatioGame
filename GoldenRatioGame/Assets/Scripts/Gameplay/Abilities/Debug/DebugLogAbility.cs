using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class DebugLogAbility : ICastAbility
    {
        private readonly string _textToLog;
        private readonly ICooldown _cooldown;

        public ICooldownReadOnly Cooldown=> _cooldown;
        public bool CanUse => !Cooldown.IsOnCooldown;

        public string Name { get; set; } = "Debug Log";
        public string Description { get; set; } = "Logs when ability is used";

        public DebugLogAbility() : this("ability has been used", new FloatCooldown(0))
        {
            
        }

        public DebugLogAbility(string textToLog, ICooldown cooldown)
        {
            _textToLog = textToLog;
            _cooldown = cooldown;
        }
        
        public bool TryCast(out ICastInfo castInfo)
        {
            castInfo = new CastInfo();
            
            if (!_cooldown.TryReset()) return false;
            
            Debug.Log(_textToLog);
         
            return true;
        }
    }
}