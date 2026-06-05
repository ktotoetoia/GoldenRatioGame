using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IM.Augments
{
    public class AugmentContainer : MonoBehaviour, IAugmentContainer
    {
        [SerializeField] private GameObject _augmentObserversSource;
        private List<IAugmentObserver> _augmentObservers;
        
        private List<IAugmentObserver> AugmentObservers => _augmentObservers ??=
            _augmentObserversSource.GetComponents<IAugmentObserver>().ToList();
        
        private readonly HashSet<IAugment> _augments = new();
        
        public IEnumerable<IAugment> Augments => _augments;

        public void Add(IAugment augment)
        {
            if (_augments.Add(augment)) AugmentObservers.ForEach(x => x.OnAdded(augment));
        } 
        public bool Remove(IAugment augment)
        {
            if (!_augments.Remove(augment)) return false;
            
            AugmentObservers.ForEach(x => x.OnRemoved(augment));
                
            return true;

        }
    }
}