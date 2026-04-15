using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class AbilityPoolModuleControllerFactory : MonoBehaviour, IFactory<IEditorObserver<IModuleEditingContext>>
    {
        public IEditorObserver<IModuleEditingContext> Create()
        {
            return new AbilityPoolModuleController();
        }
    }
}