using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentProgressManager : MonoBehaviour, IAugmentProgressManager
    {
        [SerializeField] private AugmentContainer _augmentContainer;
        private IEntity _entity;
        
        private readonly HashSet<IAugmentSource> _currentSources = new();
        private readonly Dictionary<IAugment,IAugmentFactory> _appliedFactories = new();
        
        public IReadOnlyDictionary<IAugment, IAugmentFactory> AppliedFactories => _appliedFactories;
        private void Awake()
        {
            if (!TryGetComponent(out _entity)) throw new MissingComponentException(nameof(IEntity));
        }

        public void SetActiveSources(IEnumerable<IAugmentSource> sources)
        {
            _currentSources.Clear();
            foreach (IAugmentSource source in sources)
                _currentSources.Add(source);
        }

        public void Progress(int amount)
        {
            foreach (IAugmentSource source in _currentSources)
            foreach (AugmentInfo unlocked in source.GetFor(_entity).Advance(amount))
                Apply(unlocked.Factory);
        }

        private void Apply(IAugmentFactory factory)
        {
            AddAppliedFactory(factory.Create(new AugmentContext(_entity.GameObject)),factory);
        }

        public void AddAppliedFactory(IAugment augment, IAugmentFactory factory)
        {
            _appliedFactories.Add(augment, factory);
            _augmentContainer.Add(augment);
        }
    }
}