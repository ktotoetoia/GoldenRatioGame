using UnityEngine;

namespace IM.EntityIntelligence
{
    public class TargetDistanceRangeCondition : ICondition
    {
        private readonly Transform _ownerTransform;
        private readonly TargetMemory _targetMemory;
        private readonly float _minRange;
        private readonly float _maxRange;
        
        public bool Reverse { get; set; }

        public TargetDistanceRangeCondition(Transform ownerTransform, TargetMemory targetMemory, float minRange, float maxRange)
        {
            _ownerTransform = ownerTransform;
            _targetMemory = targetMemory;
            _minRange = minRange;
            _maxRange = maxRange;
        }

        public void Start() { }
        public void Finish() { }

        public bool Check()
        {
            if (!_targetMemory.Target) return false;

            Vector3 targetPosition = _targetMemory.IsSeen 
                ? _targetMemory.Target.transform.position 
                : _targetMemory.LastSeenAt;

            float sqrDistance = (targetPosition - _ownerTransform.position).sqrMagnitude;

            float sqrMin = _minRange * _minRange;
            float sqrMax = _maxRange * _maxRange;

            if (Reverse) return sqrDistance < sqrMin || sqrDistance > sqrMax;
            
            return sqrDistance >= sqrMin && sqrDistance <= sqrMax;
        }
    }
}