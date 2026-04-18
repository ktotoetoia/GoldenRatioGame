using IM.Abilities;
using IM.LifeCycle;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.Modules
{
    public class WeaponExtension : MonoBehaviour, IWeaponExtension,IAbilityExtension, IRequireEntityExtension
    {
        [SerializeField] private GameObject _defaultWeaponGameObject;
        private IWeapon _defaultWeapon;
        private IWeapon _weapon;
        private IEntity _entity;

        public IAbilityReadOnly Ability => Weapon ?? _defaultWeapon;

        public IEntity Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                UpdateWeaponEntity();
            } 
        }

        public IWeapon Weapon
        {
            get => _weapon;
            set
            {
                _weapon = value;
                UpdateWeaponEntity();
            }
        }
        
        private void Awake()
        {
            _defaultWeapon = Instantiate(_defaultWeaponGameObject,transform).GetComponent<IWeapon>();
        }

        private void UpdateWeaponEntity()
        {
            if (_weapon is not null)
            {
                _weapon.Entity = _entity;
                    
                return;
            }

            if (_defaultWeapon is not null)
            {
                _defaultWeapon.Entity = _entity;
            }
        }
    }
}