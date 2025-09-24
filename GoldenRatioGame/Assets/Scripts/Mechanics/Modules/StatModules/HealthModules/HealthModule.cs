using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class HealthModule : MonoBehaviour, IHealthModule
    {
        //todo add capped float
        [SerializeField] private CappedValue<float> _health;
        public ICappedValue<float> Health => _health;
    }
}