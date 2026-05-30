using IM.Movement;
using UnityEngine;

namespace IM.EntityIntelligence
{
   public class WanderEntityAction : EntityAction
   {
        private readonly Transform _ownerTransform;
        private readonly IVectorMovement _movement;
        private readonly float _minWalkTime;
        private readonly float _maxWalkTime;
        private readonly float _timeBetweenWalking;
        private readonly float _biasDistance;
        private readonly System.Random _random;

        private bool _isWalking;
        private float _finishTime;
        private Vector3 _walkDirection;

        public WanderEntityAction(Transform ownerTransform, IVectorMovement movement,
            float minWalkTime, float maxWalkTime, float timeBetweenWalking, float biasDistance)
        {
            _ownerTransform     = ownerTransform;
            _movement           = movement;
            _minWalkTime        = minWalkTime;
            _maxWalkTime        = maxWalkTime;
            _timeBetweenWalking = timeBetweenWalking;
            _biasDistance       = biasDistance;
            _random             = new System.Random();
        }

        public override void OnEnter()
        {
            _isWalking          = false;
            _finishTime         = Time.time;
            _movement.Direction = Vector3.zero;
        }

        public override void Update()
        {
            if (!_isWalking)
            {
                _movement.Direction = Vector3.zero;

                if (_finishTime + _timeBetweenWalking < Time.time)
                    StartWalking();

                return;
            }

            if (_finishTime < Time.time)
            {
                _isWalking          = false;
                _movement.Direction = Vector3.zero;
                return;
            }

            _movement.Direction = _walkDirection;
        }

        public override void OnExit()
        {
            _movement.Direction = Vector3.zero;
        }

        private void StartWalking()
        {
            _isWalking     = true;
            _finishTime    = Time.time + GetRandomWalkDuration();
            _walkDirection = GetBiasedDirection();
        }

        private float GetRandomWalkDuration()
        {
            return _minWalkTime + (float)_random.NextDouble() * (_maxWalkTime - _minWalkTime);
        }

        private Vector3 GetBiasedDirection()
        {
            Vector3 toOrigin = (-_ownerTransform.position).normalized;

            Vector3 randomDirection = new Vector3(
                (float)(_random.NextDouble() * 2.0 - 1.0),
                (float)(_random.NextDouble() * 2.0 - 1.0),
                0f
            ).normalized;

            float weight = Mathf.Clamp01(_ownerTransform.position.magnitude / _biasDistance);

            return Vector3.Lerp(randomDirection, toOrigin, weight);
        }
   }
}