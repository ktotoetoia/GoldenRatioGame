using System;
using IM.Abilities;
using IM.Items;
using IM.LifeCycle;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponExtension : MonoBehaviour, IWeaponExtension, IAbilityExtension, IRequireEntityExtension
    {
        [SerializeField] private GameObject _defaultWeaponPrefab;
        private IWeapon _defaultWeapon;
        private IWeapon _equippedWeapon;
        private IEntity _entity;
        
        public event Action<IWeapon> PreferredWeaponChanged;

        public IAbilityReadOnly Ability => PreferredWeapon;

        public IEntity Entity
        {
            get => _entity;
            set 
            {
                _entity = value;
                SyncWeaponEntity();
            }
        }

        public IWeapon DefaultWeapon => _defaultWeapon;

        public IWeapon Weapon
        {
            get => _equippedWeapon;
            set
            {
                if (_equippedWeapon == value) return;

                if (_equippedWeapon is IHaveOwner haveOwner && haveOwner.Owner.Equals(this))
                {
                    haveOwner.SetOwner(null);
                }
                if (_equippedWeapon is MonoBehaviour mb && mb.transform.parent == transform)
                {
                    mb.transform.SetParent(null);
                }
                
                _equippedWeapon = value;
                
                if (_equippedWeapon is IHaveOwner newHaveOwner)
                {
                    newHaveOwner.SetOwner(this);
                }
                if (_equippedWeapon is MonoBehaviour newMb)
                {
                    newMb.transform.SetParent(transform);
                }
                
                SyncWeaponEntity();
                PreferredWeaponChanged?.Invoke(Weapon);
            }
        }

        public IWeapon PreferredWeapon => _equippedWeapon ?? _defaultWeapon;

        private void Awake()
        {
            InitializeDefaultWeapon();
        }

        private void InitializeDefaultWeapon()
        {
            if (!_defaultWeaponPrefab) return;
            
            GameObject defaultWeapon = Instantiate(_defaultWeaponPrefab, transform);
            
            if(!defaultWeapon.TryGetComponent(out _defaultWeapon)) return;
            if(defaultWeapon.TryGetComponent(out IHaveOwner owner)) owner.SetOwner(this);
            
            SyncWeaponEntity();
            PreferredWeaponChanged?.Invoke(PreferredWeapon);
        }

        private void SyncWeaponEntity()
        {
            if (PreferredWeapon != null) PreferredWeapon.Entity = _entity;
        }
    }
}