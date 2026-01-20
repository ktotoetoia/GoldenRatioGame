using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class HealthExtensionComponent : MonoBehaviour, IHealthExtension
    {
        [SerializeField] private CappedValue<float> _health;
        
        public ICappedValue<float> Health => _health;
    }
}