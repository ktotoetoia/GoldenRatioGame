using IM.Effects;
using UnityEngine;

namespace IM.Augments
{
    public class EffectAugmentObserver : MonoBehaviour, IAugmentObserver
    {
        [SerializeField] private GameObject _effectContainerSource;
        private IEffectContainer _effectContainer;
        private IEffectContainer EffectContainer => _effectContainer ??= _effectContainerSource.GetComponent<IEffectContainer>();
        
        public void OnAdded(IAugment augment)
        {
            if (augment is IEffectAugment effectAugment) EffectContainer.AddGroup(effectAugment.EffectGroup);
        }

        public void OnRemoved(IAugment augment)
        {
            if (augment is IEffectAugment effectAugment) EffectContainer.RemoveGroup(effectAugment.EffectGroup);
        }
    }
}