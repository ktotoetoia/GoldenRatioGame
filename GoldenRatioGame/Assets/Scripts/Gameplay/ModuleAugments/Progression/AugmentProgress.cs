using System.Collections.Generic;
using System.Linq;
using IM.LifeCycle;

namespace IM.Augments
{
    public class AugmentProgress : IAugmentProgress
    {
        private readonly List<AugmentInfo> _augments;
        private AugmentProgressInfo _progress;

        public IEnumerable<AugmentInfo> Augments => _augments;
        public AugmentProgressInfo Progress => _progress;
        public IEntity Entity { get; }

        public AugmentProgress(IEnumerable<AugmentInfo> augments, IEntity target, AugmentProgressInfo progress = new())
        {
            _augments = augments.ToList();
            Entity = target;
            _progress = progress;
        }

        public IEnumerable<AugmentInfo> Advance(int amount)
        {
            _progress = _progress.Add(amount);

            List<AugmentInfo> unlocked = new List<AugmentInfo>();

            while (_progress.CurrentIndex < _augments.Count && _progress.Value >= _augments[_progress.CurrentIndex].RequiredProgress)
            {
                AugmentInfo info = _augments[_progress.CurrentIndex];

                _progress = _progress.Next(_progress.Value - info.RequiredProgress);

                unlocked.Add(info);
            }

            return unlocked;
        }

        public IEnumerable<AugmentInfo> GetFinishedAugments()
        {
            return _augments.Take(_progress.CurrentIndex);
        }

        public IEnumerable<AugmentInfo> GetCurrentAugments()
        {
            if (_progress.CurrentIndex >= _augments.Count) yield break;

            yield return _augments[_progress.CurrentIndex];
        }

        public IEnumerable<AugmentInfo> GetUnstartedAugments()
        {
            return _augments.Skip(_progress.CurrentIndex + 1);
        }
    }
}