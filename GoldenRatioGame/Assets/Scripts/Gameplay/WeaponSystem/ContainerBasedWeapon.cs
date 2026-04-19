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
        private ICastAbility _castAbility;
        private IIcon _icon;
        private ItemState _itemState;
        private IIconDrawer _iconDrawer;
        private UseContext _context;

        public ICooldownReadOnly Cooldown => _castAbility.Cooldown;
        public bool CanUse => _castAbility.CanUse;
        public IEntity Entity { get; set; }
        public IStorageCell Cell { get; set; }
        public string Name => _castAbility.Name;
        public string Description => _castAbility.Description;
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
            _castAbility = GetComponent<IAbilityContainer>().Ability as ICastAbility;
            ItemState = ItemState.Show;
        }

        public bool TryCast(out ICastInfo info)
        {
            if (_castAbility.TryCast(out info))
            {
                AbilityStarted?.Invoke(_context);
                AbilityFinished?.Invoke(_context);
                
                return true;
            }

            return false;
        }
        public void UpdateAbilityUseContext(UseContext context)
        {
            _context = context;
            (_castAbility as IRequireAbilityUseContext)?.UpdateAbilityUseContext(context);
        }

        public float FocusTime => (_castAbility as IFocusPointProvider)?.FocusTime ?? 0;
        public Vector3 GetFocusPoint() => (_castAbility as IFocusPointProvider)?.GetFocusPoint() ?? default;
        public Vector3 GetFocusDirection() => (_castAbility as IFocusPointProvider)?.GetFocusDirection() ?? default;
    }
}