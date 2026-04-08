using IM.Abilities;
using IM.Graphs;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class ApplyDraftChangesEditorObserver : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private GameObject _source;
        private IAbilityPoolDraftContainer _abilityPoolDraftContainer;
        private IWeaponPoolDraftContainer _weaponPoolDraftContainer;

        private void Awake()
        {
            _abilityPoolDraftContainer = _source.GetComponent<IAbilityPoolDraftContainer>();
            _weaponPoolDraftContainer = _source.GetComponent<IWeaponPoolDraftContainer>();
        }
        
        public void OnSnapshotChanged(IModuleGraphReadOnly snapshot)
        {
            _abilityPoolDraftContainer?.CommitDraft();
            _weaponPoolDraftContainer?.CommitDraft();
        }
    }
}