using System;
using IM.Abilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class WeaponVisualView : MonoBehaviour
    {
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

        public void SetAbilityPool(IContainerAbilityPool abilityPool)
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
    }
}