using System;
using IM.Abilities;
using IM.Items;
using IM.LifeCycle;
using IM.Storages;
using IM.Values;
using UnityEngine;

namespace IM.WeaponSystem
{
    public class ContainerBasedCastWeapon : DefaultEntity, IWeapon, ICastAbility,IRequireAbilityUseContext, IFocusProvider, IAbilityEvents, IStorable, IItem
    {
        private IAbilityContainer _abilityContainer;
        private IIcon _icon;
        private IIconDrawer _iconDrawer;
        private UseContext _context;

        private ICastAbility CastAbility => _abilityContainer.Ability as ICastAbility;

        public float WindUpTime => CastAbility?.WindUpTime ?? 0;
        public ICooldownReadOnly Cooldown => CastAbility.Cooldown;
        public bool CanUse => CastAbility.CanUse;
        public IEntity Entity { get; set; }
        public IStorageCell Cell { get; set; }
        public string Name => CastAbility.Name;
        public string ShortDescription => CastAbility.ShortDescription;
        public string Description => CastAbility.Description;
        public IIcon Icon => _icon ??= new Icon(GetComponent<SpriteRenderer>().sprite);

        public object Owner { get; private set; }

        public bool SetOwner(object owner)
        {
            return !(_iconDrawer.IsDrawing = (Owner = owner) == null);
        }
        public IWeaponVisualsProvider WeaponVisualsProvider { get; private set; }
        
        public event Action<UseContext> AbilityWindUp;
        public event Action<UseContext> AbilityFired;
        
        private void Awake()
        {
            WeaponVisualsProvider = GetComponent<IWeaponVisualsProvider>();
            _iconDrawer = GetComponent<IIconDrawer>();
            _abilityContainer = GetComponent<IAbilityContainer>();
            
            if (CastAbility is IAbilityEvents events)
            {
                events.AbilityWindUp += CallWindUp;
                events.AbilityFired += CallFired;
            }
        }

        public bool TryCast(out ICastInfo info)
        {
            if (!CastAbility.TryCast(out info)) return false;
            
            return true;
        }

        private void CallWindUp(UseContext context)
        {
            AbilityWindUp?.Invoke(_context);
        }

        private void CallFired(UseContext context)
        {
            AbilityFired?.Invoke(_context);
        }
        
        public void UpdateAbilityUseContext(UseContext context)
        {
            _context = context;
            (CastAbility as IRequireAbilityUseContext)?.UpdateAbilityUseContext(context);
        }

        public float FocusTime => (CastAbility as IFocusProvider)?.FocusTime ?? 0;
        public Vector3 GetFocusPoint() => (CastAbility as IFocusProvider)?.GetFocusPoint() ?? default;
        public Vector3 GetFocusDirection() => (CastAbility as IFocusProvider)?.GetFocusDirection() ?? default;
    }
}