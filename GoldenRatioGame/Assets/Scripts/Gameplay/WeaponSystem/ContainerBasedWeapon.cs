using System;
using IM.Abilities;
using IM.Items;
using IM.LifeCycle;
using IM.Storages;
using IM.Values;
using UnityEngine;

namespace IM.WeaponSystem
{
    public class ContainerBasedWeapon : DefaultEntity, IWeapon, ICastAbility,IRequireAbilityUseContext, IFocusPointProvider, IAbilityEvents, IStorable, IItem
    {
        private IAbilityContainer _abilityContainer;
        private IIcon _icon;
        private ItemState _itemState;
        private IIconDrawer _iconDrawer;
        private UseContext _context;

        private ICastAbility CastAbility => _abilityContainer.Ability as ICastAbility;
        
        public ICooldownReadOnly Cooldown => CastAbility.Cooldown;
        public bool CanUse => CastAbility.CanUse;
        public IEntity Entity { get; set; }
        public IStorageCell Cell { get; set; }
        public string Name => CastAbility.Name;
        public string Description => CastAbility.Description;
        public IIcon Icon => _icon ??= new Icon(GetComponent<SpriteRenderer>().sprite);
        public ItemState ItemState
        {
            get => _itemState;
            set => _iconDrawer.IsDrawing = (_itemState = value) == ItemState.Show;
        }

        public IWeaponVisualsProvider WeaponVisualsProvider { get; private set; }
        
        public event Action<UseContext> AbilityStarted;
        public event Action<UseContext> AbilityFinished;
        
        private void Awake()
        {
            WeaponVisualsProvider = GetComponent<IWeaponVisualsProvider>();
            _iconDrawer = GetComponent<IIconDrawer>();
            _abilityContainer = GetComponent<IAbilityContainer>();
            ItemState = ItemState.Show;
        }

        public bool TryCast(out ICastInfo info)
        {
            if (!CastAbility.TryCast(out info)) return false;
            
            AbilityStarted?.Invoke(_context);
            AbilityFinished?.Invoke(_context);
                
            return true;
        }
        public void UpdateAbilityUseContext(UseContext context)
        {
            _context = context;
            (CastAbility as IRequireAbilityUseContext)?.UpdateAbilityUseContext(context);
        }

        public float FocusTime => (CastAbility as IFocusPointProvider)?.FocusTime ?? 0;
        public Vector3 GetFocusPoint() => (CastAbility as IFocusPointProvider)?.GetFocusPoint() ?? default;
        public Vector3 GetFocusDirection() => (CastAbility as IFocusPointProvider)?.GetFocusDirection() ?? default;
    }
}