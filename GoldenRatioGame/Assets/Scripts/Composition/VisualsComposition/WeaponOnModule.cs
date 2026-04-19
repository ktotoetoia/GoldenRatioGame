using IM.LifeCycle;
using IM.Modules;
using IM.Visuals;
using IM.WeaponSystem;
using UnityEngine;

namespace IM
{
    public class WeaponOnModule : MonoBehaviour, IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private Transform _anchor;
        private IWeaponExtension _weaponExtension;
        private IWeapon _currentWeapon;
        private IWeaponVisual _weaponVisual;
        private IModuleVisualObject _moduleVisualObject;

        private bool _visible = true;

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _moduleVisualObject = moduleVisualObject;

            if (!moduleVisualObject.Owner.Extensions.TryGet(out _weaponExtension))
                return;

            _weaponExtension.WeaponChanged += OnWeaponChanged;
            ApplyWeapon(_weaponExtension.Weapon);
        }

        private void OnDestroy()
        {
            if (_weaponExtension != null)
                _weaponExtension.WeaponChanged -= OnWeaponChanged;
        }

        private void Update()
        {
            if (_weaponVisual == null) return;

            _weaponVisual.Transform.Position = _anchor.position;
            _weaponVisual.Transform.Rotation = _anchor.rotation;
            _weaponVisual.Transform.LocalScale = _anchor.lossyScale;

            _weaponVisual.Order = _moduleVisualObject.Order;
        }

        private void OnWeaponChanged(IWeapon weapon)
        {
            ApplyWeapon(weapon);
        }

        private void ApplyWeapon(IWeapon weapon)
        {
            ReleaseCurrentVisual();

            _currentWeapon = weapon;

            if (!_visible || weapon == null)
                return;

            _weaponVisual = weapon.WeaponVisualsProvider.WeaponVisualsPool.Get();
        }

        private void ReleaseCurrentVisual()
        {
            if (_currentWeapon == null || _weaponVisual == null)
                return;

            _currentWeapon.WeaponVisualsProvider.WeaponVisualsPool.Release(_weaponVisual);
            _weaponVisual = null;
        }

        public void OnRelease()
        {
            _visible = false;
            ReleaseCurrentVisual();
        }

        public void OnGet()
        {
            _visible = true;

            if (_weaponExtension != null)
                ApplyWeapon(_weaponExtension.Weapon);
        }
    }
}