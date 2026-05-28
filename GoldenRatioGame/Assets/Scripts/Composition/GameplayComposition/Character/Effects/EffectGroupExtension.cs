using UnityEngine;

namespace IM.Effects
{
    [DisallowMultipleComponent]
    public class EffectGroupExtension : MonoBehaviour, IEffectGroupExtension
    {
        private IEffectGroup _effectGroup;
        
        public IEffectGroup EffectGroup => _effectGroup??= new EffectGroup(GetComponents<IEffectModifier>());
    }
}