using System;
using UnityEngine;

namespace IM.Values
{
    [Serializable]
    public class SpeedModifier : ISpeedModifier
    {
        [field:SerializeField] public float Add { get; set; }
        [field: SerializeField] public float Multiplication { get; set; }

        public SpeedModifier(float add = 0, float multiplication = 1)
        {
            Add = add;
            Multiplication = multiplication;
        }
    }
}