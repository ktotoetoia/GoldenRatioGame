using System;
using IM.Movement;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class MoveTowardsTargetFactory : IEntityActionFactory
    {
        [SerializeField] private float _range = 3f; 
        [SerializeField] private float _resumeRange  = 4f;
        
        public IEntityAction Create(GameObject param1)
        {
            return new MoveTowardsTarget(param1.transform, 
                    param1.GetComponent<IVectorMovement>(),
                    param1.GetComponent<IMemoryContainer>())
                {StopRange = _range,ResumeRange = _resumeRange};
        }
    }
}