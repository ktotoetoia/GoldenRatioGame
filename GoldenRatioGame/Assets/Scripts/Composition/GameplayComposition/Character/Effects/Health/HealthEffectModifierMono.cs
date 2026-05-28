using IM.Values;
using UnityEngine;

namespace IM.Effects
{
    public class HealthEffectModifierMono : MonoBehaviour, IHealthEffectModifier
    {
        [SerializeField] private CappedValue<float> _health;

        public ICappedValue<float> Health => _health;
    }
}