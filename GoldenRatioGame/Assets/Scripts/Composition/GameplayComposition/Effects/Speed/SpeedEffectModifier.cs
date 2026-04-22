using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class SpeedEffectModifier : MonoBehaviour, ISpeedEffectModifier
    {
        [SerializeField] private SpeedModifier _speedModifier;
        
        public ISpeedModifier SpeedModifier => _speedModifier;
    }
}