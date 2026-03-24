using IM.Effects;
using IM.Values;
using UnityEngine;

namespace IM.Modules
{
    public class SpeedEffectObserver : MonoBehaviour, IEffectObserver
    {
        [SerializeField] private GameObject _speedSource;
        private IHaveSpeed _speed;

        private void Awake()
        {
            _speed = _speedSource.GetComponent<IHaveSpeed>();
        }
        
        public void OnEffectGroupAdded(IEffectGroup group)
        {
            foreach (ISpeedEffectModifier modifier in group.Modifiers.GetAll<ISpeedEffectModifier>()) _speed.Speed.AddModifier(modifier.SpeedModifier);
        }
        public void OnEffectGroupRemoved(IEffectGroup group)
        {
            foreach (ISpeedEffectModifier modifier in group.Modifiers.GetAll<ISpeedEffectModifier>()) _speed.Speed.RemoveModifier(modifier.SpeedModifier);
        }
    }
}