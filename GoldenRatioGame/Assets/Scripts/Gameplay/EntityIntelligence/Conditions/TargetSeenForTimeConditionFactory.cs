using System;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class TargetSeenForTimeConditionFactory : IConditionFactory
    {
        [SerializeField] private float _requiredTime = 1f;
        [SerializeField] private bool  _triggerWhenNotSeen;

        public ICondition Create(GameObject param1)
        {
            var memoryContainer = param1.GetComponent<IMemoryContainer>();

            if (!memoryContainer.Memories.TryGet(out TargetMemory targetMemory))
            {
                targetMemory = new TargetMemory();
                memoryContainer.Add(targetMemory);
            }

            return new TargetSeenForTimeCondition(targetMemory, _requiredTime)
            {
                TriggerWhenNotSeen = _triggerWhenNotSeen
            };
        }
    }
}