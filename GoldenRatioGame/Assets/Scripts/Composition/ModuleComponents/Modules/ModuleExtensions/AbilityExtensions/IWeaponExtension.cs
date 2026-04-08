using IM.Graphs;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public interface IWeaponExtension : IExtension
    {
        IWeapon Weapon { get; set; }
    }
    
    public class WeaponToAbilities : MonoBehaviour, IEditorObserver<IModuleGraphReadOnly>
    {
        
        
        public void OnSnapshotChanged(IModuleGraphReadOnly snapshot)
        {
            throw new System.NotImplementedException();
        }
    }
}