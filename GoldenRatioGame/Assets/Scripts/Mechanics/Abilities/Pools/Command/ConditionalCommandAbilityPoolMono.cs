using System.Collections.Generic;
using UnityEngine;

namespace IM.Abilities
{
    public class ConditionalCommandAbilityPoolMono : MonoBehaviour, IConditionalCommandAbilityPool
    {
        private IConditionalCommandAbilityPool _conditionalCommandAbilityPool;
        public IReadOnlyCollection<IAbilityReadOnly> Abilities => _conditionalCommandAbilityPool.Abilities;

        private void Awake()
        {
            _conditionalCommandAbilityPool = new ConditionalCommandAbilityPool();
        }

        public bool Contains(IAbilityReadOnly ability) => _conditionalCommandAbilityPool.Contains(ability);
        public void AddAbility(IAbilityReadOnly ability) => _conditionalCommandAbilityPool.AddAbility(ability);
        public void RemoveAbility(IAbilityReadOnly ability) => _conditionalCommandAbilityPool.RemoveAbility(ability);
        public int CommandsToUndoCount => _conditionalCommandAbilityPool.CommandsToUndoCount;
        public int CommandsToRedoCount => _conditionalCommandAbilityPool.CommandsToRedoCount;
        public bool CanUndo(int count) => _conditionalCommandAbilityPool.CanUndo(count);
        public bool CanRedo(int count) => _conditionalCommandAbilityPool.CanRedo(count);
        public void Undo(int count) => _conditionalCommandAbilityPool.Undo(count);
        public void Redo(int count) => _conditionalCommandAbilityPool.Redo(count);
    }
}