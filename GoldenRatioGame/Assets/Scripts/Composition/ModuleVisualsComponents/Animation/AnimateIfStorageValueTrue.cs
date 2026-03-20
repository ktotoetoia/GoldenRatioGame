using IM.Common;
using IM.Events;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimateIfStorageValueTrue : MonoBehaviour, IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private string _tag;
        [SerializeField] private string _parameterName;
        [SerializeField] private bool _isTrigger;
        private Animator _animator;
        private IValueStorageContainer _container;
        private IValueStorage<bool> _storage;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnModuleVisualObjectInitialized(IModuleVisualObject moduleVisualObject)
        {
            _container = moduleVisualObject.Owner.Extensions.Get<IValueStorageContainer>();
        }

        public void OnRelease()
        {
            _storage.Changed -= OnEnumChanged;
        }

        public void OnGet()
        {
            _storage = _container.GetOrCreate<bool>(_tag);
            _storage.Changed += OnEnumChanged;
        }

        private void OnEnumChanged(bool obj)
        {
            if (_isTrigger)
            {
                if(obj) _animator.SetTrigger(_parameterName);
                
                return;
            }
            
            _animator.SetBool(_parameterName, obj);
        }
    }
}