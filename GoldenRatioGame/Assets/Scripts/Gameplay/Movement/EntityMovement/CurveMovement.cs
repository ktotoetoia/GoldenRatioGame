using IM.Common;
using UnityEngine;

namespace IM.Movement
{
    public class CurveMovement : IVectorMovement
    {
        private readonly IVelocityModifier _modifier;
        private readonly AnimationCurve _curve;
        
        public Accelerator Accelerator { get; }
        public float AccelerationTime
        {
            get => Accelerator.AccelerationTime;
            set => Accelerator.AccelerationTime = value;
        }
        public ISpeed Speed { get; private set; }
        public Vector2 Direction { get; set; }
        public Vector2 MovementVelocity { get; private set; }
        
        public CurveMovement(IVelocityModifier velocityModifier, AnimationCurve curve, ISpeed speed)
        {
            _modifier = velocityModifier;
            _curve = curve;
            Speed = speed;
            Accelerator = new Accelerator();
        }
        
        public void Apply()
        {
            Accelerator.Update(Direction, Time.fixedDeltaTime);
    
            Vector2 accel = Accelerator.Acceleration;
            float rawMagnitude = accel.magnitude;
            Vector2 normalizedDir = accel.normalized;

            float curveStrength = _curve.Evaluate(rawMagnitude);

            MovementVelocity = normalizedDir * (curveStrength * Speed.FinalValue) ;

            _modifier.ChangeVelocity(new VelocityInfo(VelocityAction.Add, MovementVelocity));
        }
    }
}