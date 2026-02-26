using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    public class ControllableCurveMovement : MonoBehaviour, IControllableMovement
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _rawSpeed;
        [SerializeField] private float _accelerationTime;
        private CurveMovement _movement;

        public ISpeed Speed => _movement.Speed;
        public Vector2 Direction
        {
            get => _movement.Direction;
            set
            {
                if(Active) _movement.Direction = value;
            }
        }

        public Vector2 MovementVelocity => _movement.MovementVelocity;
        [field: SerializeField] public bool Active { get; private set; } = true;

        private void Awake()
        {
            _movement = new CurveMovement(GetComponent<IVelocityModifier>(),_curve,new Speed(_rawSpeed));
        }

        private void FixedUpdate()
        {
            _movement.AccelerationTime = _accelerationTime;
            _movement.Apply();
        }

        public void Stop()
        {
            Active = false;
            _movement.Direction = Vector2.zero;
        }

        public void Halt()
        {
            Active = false;
            _movement.Direction = Vector2.zero;
            _movement.Accelerator.Reset();
        }

        public void Activate()
        {
            Active = true;
        }
    }
}