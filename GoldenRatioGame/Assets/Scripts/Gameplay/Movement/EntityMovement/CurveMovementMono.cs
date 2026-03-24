using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    [RequireComponent(typeof(IVelocityModifier))]
    public class CurveMovementMono : MonoBehaviour, IVectorMovement
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _rawSpeed;
        [SerializeField] private float _accelerationTime;
        private CurveMovement _movement;

        public ISpeed Speed => _movement.Speed;
        public Vector2 Direction
        {
            get => _movement.Direction;
            set => _movement.Direction =  value;
        }

        public Vector2 MovementVelocity => _movement.MovementVelocity;

        private void Awake()
        {
            _movement = new CurveMovement(GetComponent<IVelocityModifier>(),_curve,new Speed(_rawSpeed));
        }

        private void FixedUpdate()
        {
            _movement.AccelerationTime = _accelerationTime;
            _movement.Apply();
        }
    }
}