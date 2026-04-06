using IM.Abilities;
using IM.Graphs;
using UnityEngine;

namespace IM.Modules
{
    public class ApplyDraftChangesEditorObserver : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        [SerializeField] private AbilityPoolMono _abilityPool;
        
        public void OnSnapshotChanged(IModuleGraphReadOnly snapshot)
        {
            _abilityPool.Commit();
        }
    }
}