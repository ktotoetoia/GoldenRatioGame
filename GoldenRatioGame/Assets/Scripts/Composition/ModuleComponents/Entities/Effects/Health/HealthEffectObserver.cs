using IM.Effects;
using IM.Health;
using UnityEngine;

namespace IM.Modules
{
    public class HealthEffectObserver : MonoBehaviour, IEffectObserver
    {
        [SerializeField] private GameObject _floatHealthValuesGroupSource;
        private IFloatHealthValuesGroup _floatHealthValuesGroup;
        
        private void Awake()
        {
            _floatHealthValuesGroup = _floatHealthValuesGroupSource.GetComponent<IFloatHealthValuesGroup>();
        }
        
        public void OnEffectGroupAdded(IEffectGroup group)
        {
            foreach (IHealthEffectModifier modifier in group.Modifiers.GetAll<IHealthEffectModifier>()) _floatHealthValuesGroup.AddHealth(modifier.Health);
        }

        public void OnEffectGroupRemoved(IEffectGroup group)
        {
            foreach (IHealthEffectModifier modifier in group.Modifiers.GetAll<IHealthEffectModifier>()) _floatHealthValuesGroup.RemoveHealth(modifier.Health);
        }
    }
}