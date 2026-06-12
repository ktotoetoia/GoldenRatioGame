using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentProgressManager : MonoBehaviour,IAugmentProgressManager
    {
        [SerializeField] private AugmentContainer _augmentContainer;
        [SerializeField] private GameObject _owner;
        private readonly HashSet<IAugmentExtension> _currentExtensions = new();
        private readonly Dictionary<IAugmentExtension, AugmentProgressInfo> _progress = new();
        private readonly List<IAugmentFactory> _appliedFactories = new();

        public IReadOnlyDictionary<IAugmentExtension, AugmentProgressInfo> AugmentProgress => _progress;
        public IEnumerable<IAugmentFactory> AppliedFactories => _appliedFactories;
        
        public void SetActiveExtensions(IEnumerable<IAugmentExtension> extensions)
        {
            _currentExtensions.Clear();
            foreach (IAugmentExtension extension in extensions)
            {
                _currentExtensions.Add(extension);
            }
        }

        public void AddToProgress(IReadOnlyDictionary<IAugmentExtension, AugmentProgressInfo> augmentProgress, IEnumerable<IAugmentFactory> appliedFactories)
        {
            foreach (var pair in augmentProgress) _progress[pair.Key] = pair.Value;
            foreach (IAugmentFactory factory in appliedFactories) Apply(factory);
        }

        public void Progress(int amount)
        {
            foreach (IAugmentExtension extension in _currentExtensions)
            {
                AugmentProgressInfo progress = GetProgress(extension).Add(amount);
                IList<AugmentInfo> augments = extension.Augments as IList<AugmentInfo> ?? extension.Augments.ToList();

                while (TryApplyNextAugment(augments, ref progress)) { }

                _progress[extension] = progress;
            }
        }

        private AugmentProgressInfo GetProgress(IAugmentExtension extension)
        {
            return _progress.TryGetValue(extension, out AugmentProgressInfo progress)
                ? progress
                : new AugmentProgressInfo(0, 0);
        }

        private bool TryApplyNextAugment(IList<AugmentInfo> augments, ref AugmentProgressInfo progress)
        {
            if (progress.CurrentIndex >= augments.Count) return false;

            AugmentInfo augment = augments[progress.CurrentIndex];

            if (progress.Value < augment.RequiredProgress) return false;

            Apply(augment.Factory);

            progress = progress.Next(progress.Value - augment.RequiredProgress);

            return true;
        }

        private void Apply(IAugmentFactory factory)
        {
            _appliedFactories.Add(factory);
            _augmentContainer.Add(factory.Create(new AugmentContext(_owner)));
        }
    }
}