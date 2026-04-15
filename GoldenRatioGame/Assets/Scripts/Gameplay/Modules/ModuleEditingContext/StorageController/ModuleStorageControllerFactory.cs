using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleStorageControllerFactory : MonoBehaviour, IFactory<IEditorObserver<IModuleEditingContext>>
    {
        public IEditorObserver<IModuleEditingContext> Create()
        {
            return new ModuleStorageController();
        }
    }
}