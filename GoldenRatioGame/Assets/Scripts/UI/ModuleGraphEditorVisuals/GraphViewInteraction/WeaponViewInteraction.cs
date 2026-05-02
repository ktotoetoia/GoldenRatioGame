using System;
using System.Linq;
using IM.Modules;
using IM.WeaponSystem;
using UnityEngine;

namespace IM.UI
{
    public class WeaponViewInteraction : ContextViewer
    {
        [SerializeField] private StorageView _weaponStorage;
        [SerializeField] private WeaponVisualView _weaponView;
        [SerializeField] private WeaponPreviewPlacer _weaponPreviewPlacer;
        private IWeaponEditingService _weaponEditingService;
        
        public Func<Vector3> GetPointerPosition { get; set; } = ()  => Vector3.zero;
        
        private void Awake()
        {
            _weaponStorage.StorageElement.ObjectInteracted += ObjectInteracted;
            _weaponStorage.StorageElement.ObjectHovered += OnHold;
            _weaponStorage.StorageElement.ObjectSelected += OnSelected;
            _weaponStorage.StorageElement.ObjectReleased += OnRelease;

            _weaponPreviewPlacer.HoverPositionSource = x => GetPointerPosition();
            _weaponView.OnContainerInteracted += ClearContainer;
        }
        
        private void OnSelected(object obj)
        {
            if (_weaponPreviewPlacer.IsPreviewing || obj is not IWeapon weapon)
            {
                return;
            }
            
            _weaponPreviewPlacer.StartPreview(weapon);
        }
        
        private void OnHold(object obj)
        {
            if(_weaponPreviewPlacer.IsPreviewing) _weaponPreviewPlacer.UpdatePreviewPosition();
        }

        private void OnRelease(object obj)
        {
            if (!_weaponPreviewPlacer.IsPreviewing) return;
            
            IWeapon weapon = _weaponPreviewPlacer.FinalizePreview();
                
            if (_weaponView.GetWeaponContainerAtPosition(GetPointerPosition()) is { } weaponContainer && weapon != null)
            {
                _weaponEditingService.SetWeapon(weaponContainer,weapon);
            }
        }
        
        private void ObjectInteracted(object obj)
        {
            if (obj is not IWeapon weapon) return;
            
            if (_weaponEditingService.ContainerAbilityPoolReadOnly.AbilityContainers.FirstOrDefault(x => x is IWeaponContainer { Weapon: null }) is IWeaponContainer weaponContainer)
            {
                _weaponEditingService.SetWeapon(weaponContainer,weapon);
            }
        }

        private void ClearContainer(IWeaponContainerReadOnly weaponContainer)
        {
            _weaponEditingService?.ClearWeapon(weaponContainer);
        }

        public override void SetContext(IModuleEditingContext moduleEditingContext)
        {
            _weaponEditingService = moduleEditingContext.Services.Get<IWeaponEditingService>();
        }
        
        public override void ClearContext()
        {
            _weaponEditingService =  null;
        }
    }
}