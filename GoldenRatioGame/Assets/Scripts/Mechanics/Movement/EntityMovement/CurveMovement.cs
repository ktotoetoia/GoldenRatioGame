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
        private Vector2 _direction;
        private Accelerator _accelerator;

        public ISpeed Speed { get; private set; }

        private void Awake()
        {
            Speed = new Speed(_rawSpeed);
            _accelerator = new Accelerator();
            _modifier = GetComponent<IVelocityModifier>();
        }

        private void FixedUpdate()
        {
            _accelerator.AccelerationTime = _accelerationTime;
            _accelerator.Update(_direction,Time.fixedDeltaTime);
            Vector2 value = new Vector2(Mathf.Sign(_accelerator.Acceleration.x) * _curve.Evaluate(_accelerator.Acceleration.x),Mathf.Sign(_accelerator.Acceleration.y)* _curve.Evaluate(_accelerator.Acceleration.y));
            
            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, value * Speed.FinalValue));
        }

        public void Move(Vector2 direction)
        {
            _direction = direction;
        }
    }
}