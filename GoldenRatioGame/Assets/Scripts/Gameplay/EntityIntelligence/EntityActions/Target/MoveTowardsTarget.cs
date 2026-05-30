using IM.Movement;
using UnityEngine;

namespace IM.EntityIntelligence
{
    public class MoveTowardsTarget : EntityAction
    {
        private readonly Transform _ownerTransform;
        private readonly IVectorMovement _movement;
        private readonly TargetMemory _targetMemory;

        public float StopRange { get; set; } = 3f;

        public float ResumeRange { get; set; } = 4;

        private bool _isMoving = true;

        public MoveTowardsTarget(Transform ownerTransform, IVectorMovement movement, IMemoryContainer ownerMemory)
        {
            _ownerTransform = ownerTransform;
            _movement = movement;

            if (ownerMemory.Memories.TryGet(out _targetMemory)) return;

            _targetMemory = new TargetMemory();
            ownerMemory.Add(_targetMemory);
        }

        public override void Update()
        {
            if (!_targetMemory.Target)
            {
                _movement.Direction = Vector3.zero;
                _isMoving = false;
                return;
            }

            Vector3 destination = _targetMemory.IsSeen
                ? _targetMemory.Target.transform.position
                : _targetMemory.LastSeenAt;

            Vector3 direction = destination - _ownerTransform.position;
            float sqrDistance = direction.sqrMagnitude;

            float sqrStopRange = StopRange * StopRange;
            float sqrResumeRange = ResumeRange * ResumeRange;

            if (_isMoving)
            {
                if (sqrDistance <= sqrStopRange)
                {
                    _isMoving = false;
                }
            }
            else
            {
                if (sqrDistance > sqrResumeRange)
                {
                    _isMoving = true;
                }
            }

            _movement.Direction = _isMoving ? direction.normalized : Vector3.zero;
        }
    }
}