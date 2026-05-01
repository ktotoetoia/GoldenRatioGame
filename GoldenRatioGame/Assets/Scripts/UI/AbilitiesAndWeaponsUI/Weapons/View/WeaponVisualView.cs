using System;
using IM.Abilities;
using IM.WeaponSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class WeaponVisualView : MonoBehaviour
    {
        [SerializeField] private Camera _uiCamera;
        private UIDocument _document;
        private WeaponVisualContainer _container;
        private IAbilityPoolReadOnly _abilityPool;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _container = _document.rootVisualElement.Q<WeaponVisualContainer>();
            _document.rootVisualElement.visible = false;
        }

        private void Update()
        {
            _container?.Update();
        }

        public void SetAbilityPool(IContainerAbilityPoolReadOnly abilityPool)
        {
            if (_abilityPool != null) throw new ArgumentException("Module Entity is already set");
            
            _container.SetContainerAbilityPool(abilityPool);
            
            _abilityPool = abilityPool;
            _document.rootVisualElement.visible = true;
        }

        public void ClearEntity()
        {
            if (_abilityPool == null) return;
            
            _container.ClearContainerAbilityPool();
            
            _abilityPool = null;
            _document.rootVisualElement.visible = false;
        }
        public IWeaponContainerReadOnly GetWeaponContainerAtPosition(Vector3 worldPosition)
        {
            foreach (var (weaponContainer, visualElement) in _container.WeaponContainers)
            {
                var bounds = visualElement.worldBound;
                bounds.position += (Vector2)_document.transform.position;
                
                if (bounds.Contains(worldPosition)) return weaponContainer;
            }
            
            return null;
        }
    }
}