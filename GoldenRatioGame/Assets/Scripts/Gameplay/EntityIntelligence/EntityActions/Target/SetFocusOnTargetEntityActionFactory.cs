using System;
using IM.Values;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class SetFocusOnTargetEntityActionFactory :  IEntityActionFactory
    {
        [SerializeField] private float _duration = 0.5f;
       
        public IEntityAction Create(GameObject param1)
        {
            return new SetFocusOnTargetEntityAction(param1.transform, param1.GetComponent<IMemoryContainer>(),
                param1.GetComponent<IFocusDirectionOverrider>()){FocusDuration = _duration};
        }
    }
}