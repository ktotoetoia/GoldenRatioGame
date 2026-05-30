using UnityEngine;

namespace IM.EntityIntelligence
{
    public class TargetSeenForTimeCondition : ICondition
    {
        private readonly TargetMemory _targetMemory;
        private readonly float _requiredTime;

        private float? _seenSinceTime;
        private float? _notSeenSinceTime;

        public bool TriggerWhenNotSeen { get; set; }

        public TargetSeenForTimeCondition(TargetMemory targetMemory, float requiredTime)
        {
            _targetMemory = targetMemory;
            _requiredTime = requiredTime;
        }

        public void Start()
        {
            _seenSinceTime    = null;
            _notSeenSinceTime = null;
        }

        public void Finish() { }

        public bool Check() => TriggerWhenNotSeen ? CheckNotSeen() : CheckSeen();

        private bool CheckSeen()
        {
            if (!_targetMemory.IsSeen)
            {
                _seenSinceTime = null;
                return false;
            }

            _seenSinceTime ??= Time.time;
            return Time.time - _seenSinceTime.Value >= _requiredTime;
        }

        private bool CheckNotSeen()
        {
            if (_targetMemory.IsSeen)
            {
                _notSeenSinceTime = null;
                return false;
            }

            _notSeenSinceTime ??= Time.time;
            return Time.time - _notSeenSinceTime.Value >= _requiredTime;
        }
    }
}