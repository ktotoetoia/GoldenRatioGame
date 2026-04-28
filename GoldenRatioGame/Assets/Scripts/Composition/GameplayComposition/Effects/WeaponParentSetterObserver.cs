using System.Linq;
using IM.Graphs;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponParentSetterObserver : MonoBehaviour, IEditorObserver<IModuleEditingContextReadOnly>
    {
        [SerializeField] private Transform _parentTransform;
        
        public void OnSnapshotChanged(IModuleEditingContextReadOnly snapshot)
        {
            foreach (IWeapon weapon in snapshot.Storage.Select(x => x.Item).OfType<IWeapon>())
            {
                (weapon as MonoBehaviour)?.transform.SetParent(_parentTransform);
            }
            
            foreach (var module in snapshot.Graph.DataModules)
            {
                if (module.Value.Extensions.TryGet(out IWeaponContainerReadOnly weaponContainer) && weaponContainer.Weapon is MonoBehaviour weaponMb)
                {
                    weaponMb.transform.SetParent(_parentTransform);
                }
            }
        }
        
    }
}