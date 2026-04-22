using System.Linq;
using IM.Graphs;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Modules
{
    public class ModuleParentSetterObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private Transform _parentTransform;
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            foreach (IDataModule<IExtensibleItem> module in snapshot.Graph.DataModules)
            {
                module.Value.GameObject.transform.SetParent(_parentTransform);
            }
            foreach (IEntity entity in snapshot.Storage.Where(x => x.Item is IEntity).Select(x => x.Item as IEntity))
            {
                entity?.GameObject.transform.SetParent(_parentTransform);
            }
        }
    }
}