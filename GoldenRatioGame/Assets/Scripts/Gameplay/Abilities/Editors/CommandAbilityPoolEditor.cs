using System;
using System.Collections.Generic;
using IM.Graphs;
using IM.LifeCycle;

namespace IM.Abilities
{
    public class CommandAbilityPoolEditor<TCommandAbilityPool> : IAbilityPoolEditor<TCommandAbilityPool> where TCommandAbilityPool : ICommandAbilityPool
    {
        private readonly TCommandAbilityPool _pool;
        private readonly List<IEditorObserver<IAbilityPoolReadOnly>> _observers = new();
        private readonly IFactory<IAccess, TCommandAbilityPool> _accessFactory;

        private IAccess _accessPool;
        private int _undoIndexAtEditStart;

        public bool IsEditing => _accessPool != null;
        public bool CanSaveChanges => IsEditing;
        public IAbilityPoolReadOnly Snapshot { get; }
        public ICollection<IEditorObserver<IAbilityPoolReadOnly>> Observers => _observers;

        public CommandAbilityPoolEditor(
            TCommandAbilityPool pool,
            IFactory<IAccess, TCommandAbilityPool> accessFactory)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
            _accessFactory = accessFactory ?? throw new ArgumentNullException(nameof(accessFactory));

            Snapshot = new AbilityPoolReadOnlyWrapper(_pool);
        }

        public TCommandAbilityPool BeginEdit()
        {
            if (IsEditing)
                throw new InvalidOperationException("Ability pool is already being edited.");

            _undoIndexAtEditStart = _pool.CommandsToUndoCount;
            _accessPool = _accessFactory.Create(_pool);
            
            return (TCommandAbilityPool)_accessPool;
        }

        public void DiscardChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Ability pool is not being edited.");

            _pool.Undo(_pool.CommandsToUndoCount - _undoIndexAtEditStart);

            EndEditing();
        }

        public bool TryApplyChanges()
        {
            if (!IsEditing)
                throw new InvalidOperationException("Ability pool is not being edited.");

            EndEditing();

            foreach (var observer in _observers)
                observer.OnSnapshotChanged(Snapshot);

            return true;
        }

        private void EndEditing()
        {
            _accessPool.CanUse = false;
            _accessPool = null;

            if (_pool is INotifyOnEditingEnded notify)
            {
                notify.OnEditingEnded();
            }
        }
    }
}