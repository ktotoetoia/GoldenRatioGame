using UnityEngine;
using UnityEngine.Pool;

namespace IM.WeaponSystem
{
    public class WeaponVisualsProvider : MonoBehaviour, IWeaponVisualsProvider
    {
        [SerializeField] private GameObject _weaponVisualPrefab;
        
        public IObjectPool<IWeaponVisual> WeaponVisualsPool { get; private set; }

        private void Awake()
        {
            WeaponVisualsPool = new ObjectPool<IWeaponVisual>(Create,OnGet,OnRelease);
        }

        private IWeaponVisual Create()
        {
            GameObject created = Instantiate(_weaponVisualPrefab);
            
            return created.GetComponent<IWeaponVisual>();
        }

        private void OnGet(IWeaponVisual weaponVisual) => weaponVisual.Visible = true;
        private void OnRelease(IWeaponVisual weaponVisual) => weaponVisual.Visible = false;
    }
}