using UnityEngine;

namespace IM.Effects
{
    [DisallowMultipleComponent]
    public class EffectGroupExtension : MonoBehaviour, IEffectGroupExtension
    {
        [SerializeField] private RestorableEffectGroupFactory _restorableEffectGroupFactory;
        private IEffectGroup _effectGroup;

        public RestorableEffectGroupFactory RestorableEffectGroupFactory => _restorableEffectGroupFactory;
        public IEffectGroup EffectGroup
        {
            get => _effectGroup ??= RestorableEffectGroupFactory.Create(new EffectContext(gameObject,gameObject));
            set => _effectGroup = value;
        }
    }
}