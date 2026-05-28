using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Effects
{
    public class EffectContainerMono : MonoBehaviour, IEffectContainer
    {
        [SerializeField] private GameObject _effectObserverSource;
        private readonly List<IEffectObserver> _effectObservers = new();
        private readonly HashSet<IEffectGroup> _groups = new();
        private readonly HashSet<ITemporaryEffectGroup> _temporaryGroups = new();
        private readonly HashSet<IUpdatableEffectGroup> _updatableGroups = new();
        
        public IEnumerable<IEffectGroup> Groups => _groups;
        
        private void Awake()
        {
            _effectObserverSource.GetComponents(_effectObservers);
        }

        private void FixedUpdate()
        {
            List<ITemporaryEffectGroup> temporaryGroups = _temporaryGroups.Where(x => x.IsFinished).ToList();

            foreach (ITemporaryEffectGroup temporaryGroup in temporaryGroups) RemoveGroup(temporaryGroup);
            foreach (IUpdatableEffectGroup updatableGroup in _updatableGroups) updatableGroup.Update();
        }

        public void AddGroup(IEffectGroup group)
        {
            if(group is IUpdatableEffectGroup upd) _updatableGroups.Add(upd);
            if(group is ITemporaryEffectGroup temporary) _temporaryGroups.Add(temporary);
            if(!_groups.Add(group)) return;
            
            foreach (IEffectObserver observer in _effectObservers)
            {
                observer.OnEffectGroupAdded(group);
            }
        }

        public void RemoveGroup(IEffectGroup group)
        {
            if(group is IUpdatableEffectGroup upd) _updatableGroups.Remove(upd);
            if(group is ITemporaryEffectGroup temporary) _temporaryGroups.Remove(temporary);
            if(!_groups.Remove(group)) return;

            foreach (IEffectObserver observer in _effectObservers)
            {
                observer.OnEffectGroupRemoved(group);
            }
        }

        public IEnumerable<T> GetModifiers<T>() => _groups.SelectMany(x => x.Modifiers.GetAll<T>());
    }
}