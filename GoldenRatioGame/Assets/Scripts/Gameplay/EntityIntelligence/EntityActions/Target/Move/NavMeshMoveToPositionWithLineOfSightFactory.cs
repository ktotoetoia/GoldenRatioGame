using System;
using IM.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class NavMeshMoveToPositionWithLineOfSightFactory : IEntityActionFactory
    {
        [SerializeField, Range(0f, 180f)] private float _angle = 20f;
        [SerializeField] private float _desiredRange   = 8f;
        [SerializeField] private float _rangeTolerance = 2f;
        [SerializeField] private float _stopDistance   = 0.5f;
        [SerializeField] private float _repickDistance = 2f;
        [SerializeField] private int   _maxSamples     = 10;


        public IEntityAction Create(GameObject param1) =>
            new NavMeshMoveToPositionWithLineOfSight(
                param1.transform,
                param1.GetComponent<IVectorMovement>(),
                param1.GetComponent<NavMeshAgent>(),
                param1.GetComponent<IMemoryContainer>()
            )
            {
                DesiredRange   = _desiredRange,
                RangeTolerance = _rangeTolerance,
                StopDistance   = _stopDistance,
                RepickDistance = _repickDistance,
                MaxSamples     = _maxSamples,
                Angle          = _angle
            };
    }
}