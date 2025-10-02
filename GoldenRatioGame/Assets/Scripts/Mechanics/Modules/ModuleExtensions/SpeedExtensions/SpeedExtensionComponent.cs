using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class SpeedExtensionComponent : MonoBehaviour, ISpeedExtension
    {
        [SerializeField] private SpeedModifier _speedModifier;
        
        public ISpeedModifier SpeedModifier => _speedModifier;
    }
}