using IM.Abilities;
using IM.Modules;
using IM.Visuals;
using UnityEngine;

namespace IM
{
    public class FocusTimeToAnimationSpeed : MonoBehaviour, IRequireModuleVisualObjectInitialization
    {
        [SerializeField] private string _floatName;
        private Animator _animator;
        private IAbilityExtension _abilityExtension;
        
        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            moduleVisualObject.Owner.Extensions.TryGet(out _abilityExtension);
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_abilityExtension?.Ability is not IFocusPointProvider focusPointProvider) return;
            
            _animator.SetFloat(_floatName, 1f / Mathf.Max(focusPointProvider.FocusTime, 0.0001f));
        }
    }
}