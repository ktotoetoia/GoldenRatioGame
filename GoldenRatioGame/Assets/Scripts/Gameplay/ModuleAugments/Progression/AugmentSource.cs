using System.Collections.Generic;
using IM.LifeCycle;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentSource : MonoBehaviour, IAugmentSource
    {
        [SerializeField] private List<AugmentInfo> _augments;
        private readonly List<IAugmentProgress> _progresses = new();

        public IEnumerable<IAugmentProgress> Progresses => _progresses;

        public void AddProgress(IEntity entity, AugmentProgressInfo progress)
        {
            _progresses.Add(new AugmentProgress(_augments,entity,progress));
        }
        
        public IAugmentProgress GetFor(IEntity entity)
        {
            foreach (IAugmentProgress progress in _progresses)
            {
                if (progress.Entity == entity) return progress;
            }

            IAugmentProgress createdProgress = new AugmentProgress(_augments,entity);
            
            _progresses.Add(createdProgress);
            
            return createdProgress;
        }
    }
}