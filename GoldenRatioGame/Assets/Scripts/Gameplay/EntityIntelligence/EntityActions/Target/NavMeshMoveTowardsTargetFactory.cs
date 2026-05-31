using System;
using IM.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class NavMeshMoveTowardsTargetFactory : IEntityActionFactory
    {
        [SerializeField] private float _range = 3f; 
        [SerializeField] private float _resumeRange  = 4f;
        
        public IEntityAction Create(GameObject param1)
        {
            return new NavMeshMoveTowardsTarget(param1.transform, 
                    param1.GetComponent<IVectorMovement>(),
                    param1.GetComponent<NavMeshAgent>(),
                    param1.GetComponent<IMemoryContainer>())
                {StopRange = _range,ResumeRange = _resumeRange};
        }
    }
}