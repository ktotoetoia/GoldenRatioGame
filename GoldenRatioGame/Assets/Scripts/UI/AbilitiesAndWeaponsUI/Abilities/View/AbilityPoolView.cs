using System;
using IM.Abilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace IM.UI
{
    public class AbilityPoolView : MonoBehaviour
    {
        private UIDocument _document;
        private AbilityPoolReadonlyContainer _container;
        private IAbilityPoolReadOnly _abilityPool;

        private void Awake()
        {
            _document = GetComponent<UIDocument>();
            _container = _document.rootVisualElement.Q<AbilityPoolReadonlyContainer>();
            _document.rootVisualElement.visible = false;
        }

        private void Update()
        {
            _container?.Update();
        }

        public void SetAbilityPool(IAbilityPoolReadOnly abilityPool)
        {
            if (_abilityPool != null) throw new ArgumentException("Module Entity is already set");
            
            _container.SetAbilityPool(abilityPool);
            
            _abilityPool = abilityPool;
            _document.rootVisualElement.visible = true;
        }

        public void ClearEntity()
        {
            if (_abilityPool == null) return;
            
            _container.ClearAbilityPool();
            
            _abilityPool = null;
            _document.rootVisualElement.visible = false;
        }
    }
}