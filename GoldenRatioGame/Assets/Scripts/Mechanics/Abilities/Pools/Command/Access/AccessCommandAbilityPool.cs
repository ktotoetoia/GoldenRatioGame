using System;
using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class AccessConditionalCommandAbilityPool : IAccessConditionalCommandAbilityPool
    {
        private readonly IConditionalCommandAbilityPool _abilityPool;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => !TryUse() ? null : _abilityPool.Abilities;

        public int CommandsToUndoCount => TryUse() ? _abilityPool.CommandsToUndoCount : -1;

        public int CommandsToRedoCount => TryUse() ? _abilityPool.CommandsToRedoCount : -1;

        public bool CanUse { get; set; }
        public bool ThrowIfCantUse { get; set; }
        
        public AccessConditionalCommandAbilityPool(IConditionalCommandAbilityPool abilityPool)
        {
            _abilityPool = abilityPool;
        }
        
        public bool Contains(IAbilityReadOnly ability)
        {
            if(!TryUse()) return false;
            return _abilityPool.Contains(ability);
        }

        public bool CanAddAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return false;
            return _abilityPool.CanAddAbility(ability);
        }

        public bool CanRemoveAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return false;
            return _abilityPool.CanRemoveAbility(ability);
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return ;
            _abilityPool.AddAbility(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            if(!TryUse()) return ;
            _abilityPool.RemoveAbility(ability);
        }

        public bool CanUndo(int count)
        {
            if(!TryUse()) return false;
            
            return _abilityPool.CanUndo(count);
        }

        public bool CanRedo(int count)
        {
            if(!TryUse()) return false;

            return _abilityPool.CanRedo(count);
        }

        public void Undo(int count)
        {
            if(!TryUse()) return;

            _abilityPool.Undo(count);
        }

        public void Redo(int count)
        {
            if(!TryUse()) return;
            
            _abilityPool.Redo(count);
        }

        private bool TryUse()
        {
            if (CanUse) return true;

            if (ThrowIfCantUse)
                throw new InvalidOperationException("Ability Pool access denied (CanUse = false).");
            
            Debug.LogWarning("Ability Pool access denied (CanUse = false).");

            return false;
        }
    }
}