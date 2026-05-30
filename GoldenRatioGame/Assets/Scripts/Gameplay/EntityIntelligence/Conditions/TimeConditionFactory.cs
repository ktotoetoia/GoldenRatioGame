using System;
using UnityEngine;

namespace IM.EntityIntelligence
{
    [Serializable]
    public class TimeConditionFactory : IConditionFactory
    {
        [SerializeField] private float _requiredTime = 0.5f;
        
        public  ICondition Create(GameObject param1) => new TimeCondition(_requiredTime);
    }
}