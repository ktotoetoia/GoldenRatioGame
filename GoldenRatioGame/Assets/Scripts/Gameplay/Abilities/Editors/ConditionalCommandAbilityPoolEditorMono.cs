using System.Collections.Generic;
using IM.Graphs;
using UnityEngine;

namespace IM.Abilities
{
    public class ConditionalCommandAbilityPoolEditorMono : MonoBehaviour, IAbilityPoolEditor<IConditionalCommandAbilityPool>
    {
        private IAbilityPoolEditor<IConditionalCommandAbilityPool> _abilityPoolEditor;
        
        public IAbilityPoolReadOnly Snapshot => _abilityPoolEditor.Snapshot;
        public ICollection<IEditorObserver<IAbilityPoolReadOnly>> Observers => _abilityPoolEditor.Observers;
        public bool IsEditing => _abilityPoolEditor.IsEditing;
        public bool CanSaveChanges => _abilityPoolEditor.CanSaveChanges;

        private void Awake()
        {
            _abilityPoolEditor = new CommandAbilityPoolEditor<IConditionalCommandAbilityPool>(GetComponent<IConditionalCommandAbilityPool>(),new AccessConditionalCommandAbilityPoolFactory());
        }

        public IConditionalCommandAbilityPool BeginEdit() => _abilityPoolEditor.BeginEdit();
        public void DiscardChanges() => _abilityPoolEditor.DiscardChanges();
        public bool TryApplyChanges() => _abilityPoolEditor.TryApplyChanges();
    }
}