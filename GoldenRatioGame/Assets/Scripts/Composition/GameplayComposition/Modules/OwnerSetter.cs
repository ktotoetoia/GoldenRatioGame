using System.Collections.Generic;
using IM.Graphs;
using IM.Items;
using UnityEngine;

namespace IM.Modules
{
    public class OwnerSetter : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        private List<IHaveOwner> _owned;
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            throw new System.NotImplementedException();
        }
    }
}