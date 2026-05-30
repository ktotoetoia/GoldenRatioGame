using UnityEngine;

namespace IM.EntityIntelligence
{
    public class TimeCondition : ICondition
    {
        private float _startTime;

        public float RequiredTime { get; set; }

        public TimeCondition(float requiredTime = 1f)
        {
            RequiredTime = requiredTime;
        }

        public void Start()
        {
            _startTime = Time.time;
        }

        public void Finish() { }

        public virtual bool Check()
        {
            return Time.time - _startTime >= RequiredTime;
        }
    }
}