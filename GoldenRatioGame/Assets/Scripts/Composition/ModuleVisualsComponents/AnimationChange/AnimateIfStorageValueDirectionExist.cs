using IM.Common;
using IM.Events;
using UnityEngine;

namespace IM.Visuals
{
    public class AnimateIfStorageValueDirectionExist : MonoBehaviour, IRequireModuleVisualObjectInitialization, IPoolObject
    {
        [SerializeField] private string _tag;
        [SerializeField] private string _parameterName;
        private Animator _animator;
        private IValueStorageContainer _container;
        private IValueStorage<Direction> _storage;

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
            _storage = _container.GetOrCreate<Direction>(_tag);
            _storage.Changed += OnEnumChanged;
        }

        private void OnEnumChanged(Direction obj)
        {
            _animator.SetBool(_parameterName, obj != Direction.None);
        }
    }
}