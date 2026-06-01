using System;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class TargetDistanceRangeConditionFactory : IConditionFactory
    {
        [SerializeField] private float _minRange = 0f;
        [SerializeField] private float _maxRange = 10f;
        [SerializeField] private bool _reverse = false;

        public ICondition Create(GameObject param1)
        {
            var memoryContainer = param1.GetComponent<IMemoryContainer>();

            if (!memoryContainer.Memories.TryGet(out TargetMemory targetMemory))
            {
                targetMemory = new TargetMemory();
                memoryContainer.Add(targetMemory);
            }

            return new TargetDistanceRangeCondition(param1.transform, targetMemory, _minRange, _maxRange){Reverse = _reverse};
        }
    }
}