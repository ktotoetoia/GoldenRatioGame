using System;
using IM.Items;
using IM.Values;
using UnityEngine;

namespace IM.Abilities
{
    public class CastAbility : ICastAbility, IHaveIcon, IAbilityEvents, IRequireAbilityUseContext, ITickable, IInterruptable
    {
        protected readonly ICooldown _cooldown;
        protected UseContext _context;
        protected bool _isWindingUp;
        protected float _windUpEndTime;
        protected CastInfo _castInfo;

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public IIcon Icon { get; set; }

        public float WindUpTime { get; set; } = 0.3f;
        public ICooldownReadOnly Cooldown => _cooldown;
        public bool CanUse => !Cooldown.IsOnCooldown && !_isWindingUp;

        public event Action<UseContext> AbilityWindUp;
        public event Action<UseContext> AbilityFired;

        public CastAbility(float cooldown) : this(new FloatCooldown(cooldown)) {}

        public CastAbility(ICooldown cooldown)
        {
            _cooldown = cooldown;
        }

        private void BeginWindUp()
        {
            _isWindingUp = true;
            _windUpEndTime = Time.time + WindUpTime;

            AbilityWindUp?.Invoke(_context);
        }

        public void Tick()
        {
            if (_isWindingUp && Time.time >= _windUpEndTime)
            {
                CompleteWindUp();
            }
        }

        private void CompleteWindUp()
        {
            _isWindingUp = false;

            if (!_cooldown.TryReset()) 
            {
                Debug.LogWarning($"Ability {Name} failed to trigger cooldown.");
                return; 
            }
            
            OnWindUpComplete(_context);
            _castInfo.Completed = true;
            AbilityFired?.Invoke(_context);
        }

        protected virtual void OnWindUpComplete(UseContext context) {}

        public bool TryCast(out ICastInfo info)
        {
            if (!CanUse)
            {
                info = null;
                return false;
            }
            
            _castInfo = new CastInfo(false);
            info = _castInfo;
            
            BeginWindUp();

            return true;
        }

        public void UpdateAbilityUseContext(UseContext context) => _context = context;

        public bool CanInterrupt(InterruptionCause cause) => _isWindingUp && cause == InterruptionCause.Forced;

        public bool TryInterrupt(InterruptionCause cause)
        {
            if (CanInterrupt(cause))
            {
                Interrupt(cause);
                
                return true;
            }
            
            return false;
        }

        public void Interrupt(InterruptionCause cause)
        {
            _isWindingUp = false;
        }
    }
}