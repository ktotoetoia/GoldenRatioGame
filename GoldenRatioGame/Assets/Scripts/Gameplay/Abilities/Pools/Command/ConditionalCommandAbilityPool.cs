using System;
using System.Collections.Generic;
using IM.Graphs;

namespace IM.Abilities
{
    public class ConditionalCommandAbilityPool : IConditionalCommandAbilityPool, INotifyOnEditingEnded
    {
        private readonly ICommandAbilityPool _commandAbilityPool;
        private readonly IAbilityPoolConditions _abilityPoolConditions;
        
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _commandAbilityPool.Abilities;
        public int CommandsToUndoCount => _commandAbilityPool.CommandsToUndoCount;
        public int CommandsToRedoCount => _commandAbilityPool.CommandsToRedoCount;

        public ConditionalCommandAbilityPool() : this(new CommandAbilityPool())
        {
            
        }

        public ConditionalCommandAbilityPool(ICommandAbilityPool commandAbilityPool) : this(commandAbilityPool, new DefaultAbilityPoolConditions())
        {
            
        }
        
        public ConditionalCommandAbilityPool(ICommandAbilityPool commandAbilityPool, IAbilityPoolConditions abilityPoolConditions)
        {
            _commandAbilityPool = commandAbilityPool;
            _abilityPoolConditions = abilityPoolConditions;
        }

        public void AddAbility(IAbilityReadOnly ability)
        {
            if (!CanAddAbility(ability)) throw new InvalidOperationException("Can't add ability (condition failed)");
            
            _commandAbilityPool.AddAbility(ability);
        }

        public void RemoveAbility(IAbilityReadOnly ability)
        {
            if(!CanRemoveAbility(ability)) throw new InvalidOperationException("Can't remove ability (condition failed)");
            
            _commandAbilityPool.RemoveAbility(ability);
        }
        
        public bool CanAddAbility(IAbilityReadOnly ability) => _abilityPoolConditions.CanAddAbility(ability);
        public bool CanRemoveAbility(IAbilityReadOnly ability) => _abilityPoolConditions.CanRemoveAbility(ability);

        public bool Contains(IAbilityReadOnly ability)
        {
            return _commandAbilityPool.Contains(ability);
        }

        public bool CanUndo(int count) => _commandAbilityPool.CanUndo(count);
        public bool CanRedo(int count) => _commandAbilityPool.CanRedo(count);
        public void Undo(int count) => _commandAbilityPool.Undo(count);
        public void Redo(int count) => _commandAbilityPool.Redo(count);

        public void OnEditingEnded()
        {
            if (_commandAbilityPool is INotifyOnEditingEnded ntf)
            {
                ntf.OnEditingEnded();
            }
        }
    }
}