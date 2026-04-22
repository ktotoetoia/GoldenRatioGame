using IM.Effects;
using UnityEngine;

namespace IM.Modules
{
    [DisallowMultipleComponent]
    public class EffectGroupExtension : MonoBehaviour, IEffectGroupExtension
    {
        private IEffectGroup _effectGroup;
        
        public IEffectGroup EffectGroup => _effectGroup??= new EffectGroup(GetComponents<IEffectModifier>());
    }
}