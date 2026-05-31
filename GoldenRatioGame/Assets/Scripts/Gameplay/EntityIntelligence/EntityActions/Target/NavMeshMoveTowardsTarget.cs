using IM.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace IM.EntityIntelligence
{
    public class NavMeshMoveTowardsTarget : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IVectorMovement _movement;
        private readonly NavMeshAgent _agent;
        private readonly TargetMemory _targetMemory;

        public float StopRange { get; set; } = 3f;
        public float ResumeRange { get; set; } = 4f;

        private bool _isMoving = true;

        public NavMeshMoveTowardsTarget(
            Transform ownerTransform, 
            IVectorMovement movement, 
            NavMeshAgent agent, 
            IMemoryContainer ownerMemory)
        {
            _ownerTransform = ownerTransform;
            _movement = movement;
            _agent = agent;

            _agent.updatePosition = false;
            _agent.updateRotation = false;

            if (ownerMemory.Memories.TryGet(out _targetMemory)) return;

            _targetMemory = new TargetMemory();
            ownerMemory.Add(_targetMemory);
        }

        public override void Update()
        {
            if(!_agent.isOnNavMesh) return;

            _agent.nextPosition = _ownerTransform.position;

            if (!_targetMemory.Target)
            {
                _movement.Direction = Vector3.zero;
                _isMoving = false;
                if (_agent.hasPath) _agent.ResetPath();
                return;
            }

            Vector3 destination = _targetMemory.IsSeen
                ? _targetMemory.Target.transform.position
                : _targetMemory.LastSeenAt;
            
            _agent.SetDestination(destination);

            Vector3 directDirection = destination - _ownerTransform.position;
            float sqrDistance = directDirection.sqrMagnitude;

            float sqrStopRange = StopRange * StopRange;
            float sqrResumeRange = ResumeRange * ResumeRange;

            if (_isMoving)
            {
                if (sqrDistance <= sqrStopRange)
                {
                    _isMoving = false;
                    _agent.ResetPath();
                }
            }
            else
            {
                if (sqrDistance > sqrResumeRange)
                {
                    _isMoving = true;
                }
            }

            if (_isMoving)
            {
                Vector3 navDirection = _agent.desiredVelocity.normalized;

                if (navDirection.sqrMagnitude == 0f)
                {
                    navDirection = (_agent.steeringTarget - _ownerTransform.position).normalized;
                }

                _movement.Direction = navDirection;
            }
            else
            {
                _movement.Direction = Vector3.zero;
            }
        }
    }
}