using System.Collections.Generic;
using UnityEngine;

namespace IM.Effects
{
    public class EffectContainerMono : MonoBehaviour, IEffectContainer
    {
        [SerializeField] private GameObject _effectObserverSource;
        private readonly IEffectContainer _effectContainer = new EffectContainer();
        private readonly List<IEffectObserver> _effectObservers = new();
        
        public IEnumerable<IEffectGroup> Groups => _effectContainer.Groups;

        private void Awake()
        {
            _effectObserverSource.GetComponents(_effectObservers);
        }

        public void AddGroup(IEffectGroup group)
        {
            _effectContainer.AddGroup(group);

            foreach (IEffectObserver observer in _effectObservers)
            {
                observer.OnEffectGroupAdded(group);
            }
        }

        public void RemoveGroup(IEffectGroup group)
        {
            _effectContainer.RemoveGroup(group);
        
            foreach (IEffectObserver observer in _effectObservers)
            {
                observer.OnEffectGroupRemoved(group);
            }
        }

        public IEnumerable<T> GetModifiers<T>()
        {
            return _effectContainer.GetModifiers<T>();
        }
    }
}