using System;
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
        
        public Func<Vector3> GetPointerPosition { get; set; } = ()  => Vector3.zero;
        
        private void Awake()
        {
            _weaponStorage.StorageElement.ObjectInteracted += ObjectInteracted;
            _weaponStorage.StorageElement.ObjectHovered += OnHold;
            _weaponStorage.StorageElement.ObjectSelected += OnSelected;
            _weaponStorage.StorageElement.ObjectReleased += OnRelease;

            _weaponPreviewPlacer.HoverPositionSource = x => GetPointerPosition();
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
            if (_weaponPreviewPlacer.IsPreviewing)
            {
                _weaponPreviewPlacer.FinalizePreview();
            }
        }
        
        private void ObjectInteracted(object obj)
        {
            if(obj is IWeapon weapon);
        }

        public override void SetContext(IModuleEditingContext moduleEditingContext)
        {
            
        }
        
        public override void ClearContext()
        {
            
        }
    }
}