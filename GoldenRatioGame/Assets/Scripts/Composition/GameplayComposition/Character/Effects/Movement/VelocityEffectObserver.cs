using System.Collections.Generic;
using System.Linq;
using IM.Movement;
using UnityEngine;

namespace IM.Effects
{
    public class VelocityEffectObserver : MonoBehaviour,IEffectObserver
    {
        [SerializeField] private GameObject _velocityModifierSource;
        private readonly List<IEffectGroup> _groups = new();
        private IVelocityModifier _velocityModifier;
        
        private void Awake()
        {
            _velocityModifier = _velocityModifierSource.GetComponent<IVelocityModifier>();
        }
        
        private void FixedUpdate()
        {
            foreach(IVelocityEffectModifier modifier in _groups.SelectMany(x => x.Modifiers.GetAll<IVelocityEffectModifier>()))
            {
                _velocityModifier.ChangeVelocity(new VelocityInfo(modifier.Velocity));
            }
        }
        
        public void OnEffectGroupAdded(IEffectGroup group)
        {
            if(group.Modifiers.Collection.Any(x => x is IVelocityEffectModifier))
            {
                _groups.Add(group);
            }
        }

        public void OnEffectGroupRemoved(IEffectGroup group)
        {
            _groups.Remove(group);
        }
    }
}