using IM.Movement;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace IM.EntityIntelligence
{
   public class NavMeshMoveToPositionWithLineOfSight : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IVectorMovement _movement;
        private readonly NavMeshAgent _agent;
        private readonly TargetMemory _targetMemory;

        public float DesiredRange    { get; set; } = 8f;
        public float RangeTolerance  { get; set; } = 2f;
        public float StopDistance    { get; set; } = 0.5f;
        public float RepickDistance  { get; set; } = 2f;
        public float Angle           { get; set; } = 180f;
        public int   MaxSamples      { get; set; } = 10;

        private const int EnvironmentLayer = 9;

        private Vector3 _positionTarget;
        private Vector3 _lastTargetPosition;
        private bool    _hasPositionTarget;
        private bool    _isMoving;

        public NavMeshMoveToPositionWithLineOfSight(
            Transform ownerTransform,
            IVectorMovement movement,
            NavMeshAgent agent,
            IMemoryContainer ownerMemory)
        {
            _ownerTransform = ownerTransform;
            _movement       = movement;
            _agent          = agent;

            _agent.updatePosition = false;
            _agent.updateRotation = false;

            if (ownerMemory.Memories.TryGet(out _targetMemory)) return;
            _targetMemory = new TargetMemory();
            ownerMemory.Add(_targetMemory);
        }

        public override void OnEnter()
        {
            _hasPositionTarget = false;
            _isMoving          = false;
        }

        public override void Update()
        {
            if (!_agent.isOnNavMesh) return;

            _agent.nextPosition = _ownerTransform.position;

            if (!_targetMemory.Target)
            {
                _movement.Direction = Vector3.zero;
                if (_agent.hasPath) _agent.ResetPath();
                _hasPositionTarget = false;
                return;
            }

            Vector3 targetPosition = _targetMemory.Target.transform.position;

            bool targetMoved = (_lastTargetPosition - targetPosition).sqrMagnitude
                               > RepickDistance * RepickDistance;

            if (!_hasPositionTarget || targetMoved)
            {
                if (TryPickPosition(targetPosition, out Vector3 picked))
                {
                    _positionTarget     = picked;
                    _hasPositionTarget  = true;
                    _lastTargetPosition = targetPosition;
                    _agent.SetDestination(_positionTarget);
                    _isMoving = true;
                }
            }

            if (!_hasPositionTarget)
            {
                _movement.Direction = Vector3.zero;
                return;
            }

            float sqrDistanceToTarget = (_positionTarget - _ownerTransform.position).sqrMagnitude;

            if (_isMoving && sqrDistanceToTarget <= StopDistance * StopDistance)
            {
                _isMoving = false;
                _agent.ResetPath();
            }

            if (!_isMoving)
            {
                _movement.Direction = Vector3.zero;
                return;
            }

            Vector3 navDirection = _agent.desiredVelocity.normalized;

            if (navDirection.sqrMagnitude == 0f)
                navDirection = (_agent.steeringTarget - _ownerTransform.position).normalized;

            _movement.Direction = navDirection;
        }

        public override void OnExit()
        {
            _movement.Direction = Vector3.zero;
            if (_agent.hasPath) _agent.ResetPath();
        }

        private bool TryPickPosition(Vector3 targetPosition, out Vector3 result)
        {
            int environmentMask = 1 << EnvironmentLayer;
            float minRange = Mathf.Max(0f, DesiredRange - RangeTolerance);
            float maxRange = DesiredRange + RangeTolerance;
            float halfAngleRad = (Angle * 0.5f) * Mathf.Deg2Rad;

            Vector3 toOwner = _ownerTransform.position - targetPosition;
            toOwner.z = 0f;

            if (toOwner.sqrMagnitude < 0.0001f)
                toOwner = _ownerTransform.up; // or right, just a stable fallback

            toOwner.Normalize();
            float centerAngle = Mathf.Atan2(toOwner.y, toOwner.x);

            for (int i = 0; i < MaxSamples; i++)
            {
                float sampleAngle = Random.Range(centerAngle - halfAngleRad, centerAngle + halfAngleRad);
                float distance = Random.Range(minRange, maxRange);

                Vector3 candidate = targetPosition + new Vector3(
                    Mathf.Cos(sampleAngle) * distance,
                    Mathf.Sin(sampleAngle) * distance,
                    0f
                );

                Debug.DrawLine(_ownerTransform.position, candidate, Color.blue, 2);

                if (!NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    continue;

                Vector3 navPoint = hit.position;

                Vector3 rayStart = navPoint;
                Vector3 rayEnd = targetPosition;
                Vector3 directionToTarget = rayEnd - rayStart;

                if (Physics.Raycast(rayStart, directionToTarget.normalized, directionToTarget.magnitude, environmentMask))
                    continue;

                result = navPoint;
                Debug.DrawLine(_ownerTransform.position, navPoint, Color.red, 2);
                return true;
            }

            result = Vector3.zero;
            return false;
        }
    }
}