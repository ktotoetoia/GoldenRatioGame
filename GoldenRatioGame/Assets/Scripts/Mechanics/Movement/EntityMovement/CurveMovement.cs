using IM.Values;
using UnityEngine;

namespace IM.Movement
{
    public class CurveMovement : MonoBehaviour, IVectorMovement
    {
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private float _rawSpeed;
        [SerializeField] private float _accelerationTime;
        private IVelocityModifier _modifier;
        private Accelerator _accelerator;

        public ISpeed Speed { get; private set; }
        public Vector2 MovementDirection { get; private set; }
        public Vector2 MovementVelocity { get; private set; }

        private void Awake()
        {
            Speed = new Speed(_rawSpeed);
            _accelerator = new Accelerator();
            _modifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            _accelerator.AccelerationTime = _accelerationTime;
            _accelerator.Update(MovementDirection,Time.fixedDeltaTime);
            Vector2 acceleration = _accelerator.Acceleration;

            Vector2 value = new Vector2(_curve.Evaluate(acceleration.x), _curve.Evaluate(acceleration.y)) *
                            new Vector2(acceleration.normalized.x, acceleration.normalized.y);

            MovementVelocity = value * Speed.FinalValue;
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, MovementVelocity));
        }

        public void Move(Vector2 direction)
        {
            MovementDirection = direction;
        }
    }
}